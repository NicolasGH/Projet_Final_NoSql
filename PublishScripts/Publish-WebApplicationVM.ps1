#Requires -Version 3.0

<#
.SYNOPSIS
Crée et déploie une machine virtuelle Microsoft Azure pour un projet web Visual Studio.
Pour plus de détails, visitez le site à l'adresse suivante : http://go.microsoft.com/fwlink/?LinkID=394472 

.EXAMPLE
PS C:\> .\Publish-WebApplicationVM.ps1 `
-Configuration .\Configurations\WebApplication1-VM-dev.json `
-WebDeployPackage ..\WebApplication1\WebApplication1.zip `
-VMPassword @{Name = "admin"; Password = "password"} `
-AllowUntrusted `
-Verbose


#>
[CmdletBinding(HelpUri = 'http://go.microsoft.com/fwlink/?LinkID=391696')]
param
(
    [Parameter(Mandatory = $true)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $Configuration,

    [Parameter(Mandatory = $false)]
    [String]
    $SubscriptionName,

    [Parameter(Mandatory = $false)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $WebDeployPackage,

    [Parameter(Mandatory = $false)]
    [Switch]
    $AllowUntrusted,

    [Parameter(Mandatory = $false)]
    [ValidateScript( { $_.Contains('Name') -and $_.Contains('Password') } )]
    [Hashtable]
    $VMPassword,

    [Parameter(Mandatory = $false)]
    [ValidateScript({ !($_ | Where-Object { !$_.Contains('Name') -or !$_.Contains('Password')}) })]
    [Hashtable[]]
    $DatabaseServerPassword,

    [Parameter(Mandatory = $false)]
    [Switch]
    $SendHostMessagesToOutput = $false
)


function New-WebDeployPackage
{
    #Écrire une fonction pour générer et empaqueter votre application web

    #Pour générer votre application web, utilisez MsBuild.exe. Pour obtenir de l'aide, consultez les informations de référence relatives à la syntaxe de ligne de commande de MSBuild à l'adresse suivante : http://go.microsoft.com/fwlink/?LinkId=391339
}

function Test-WebApplication
{
    #Modifier cette fonction pour exécuter des tests unitaires sur votre application web

    #Pour écrire une fonction permettant d'exécuter des tests unitaires sur votre application web, utilisez VSTest.Console.exe. Pour obtenir de l'aide, consultez les informations de référence relatives à la syntaxe de ligne de commande de VSTest.Console à l'adresse suivante : http://go.microsoft.com/fwlink/?LinkId=391340
}

function New-AzureWebApplicationVMEnvironment
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Configuration,

        [Parameter (Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $VMPassword,

        [Parameter (Mandatory = $false)]
        [AllowNull()]
        [Hashtable[]]
        $DatabaseServerPassword
    )
   
    $VMInfo = New-AzureVMEnvironment `
        -CloudServiceConfiguration $Config.cloudService `
        -VMPassword $VMPassword

    # Créez les bases de données SQL. La chaîne de connexion est utilisée pour le déploiement.
    $connectionString = New-Object -TypeName Hashtable
    
    if ($Config.Contains('databases'))
    {
        @($Config.databases) |
            Where-Object {$_.connectionStringName -ne ''} |
            Add-AzureSQLDatabases -DatabaseServerPassword $DatabaseServerPassword |
            ForEach-Object { $connectionString.Add($_.Name, $_.ConnectionString) }           
    }
    
    return @{ConnectionString = $connectionString; VMInfo = $VMInfo}   
}

function Publish-AzureWebApplicationToVM
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Config,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $ConnectionString,

        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $WebDeployPackage,
        
        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $VMInfo           
    )
    $waitingTime = $VMWebDeployWaitTime

    $result = $null
    $attempts = 0
    $allAttempts = 60
    do 
    {
        $result = Publish-WebPackageToVM `
            -VMDnsName $VMInfo.VMUrl `
            -IisWebApplicationName $Config.webDeployParameters.IisWebApplicationName `
            -WebDeployPackage $WebDeployPackage `
            -UserName $VMInfo.UserName `
            -UserPassword $VMInfo.Password `
            -AllowUntrusted:$AllowUntrusted `
            -ConnectionString $ConnectionString
         
        if ($result)
        {
            Write-VerboseWithTime ($scriptName + ' Réussite de la publication vers la machine virtuelle.')
        }
        elseif ($VMInfo.IsNewCreatedVM -and !$Config.cloudService.virtualMachine.enableWebDeployExtension)
        {
            Write-VerboseWithTime ($scriptName + ' Vous devez affecter la valeur $true à "enableWebDeployExtension".')
        }
        elseif (!$VMInfo.IsNewCreatedVM)
        {
            Write-VerboseWithTime ($scriptName + ' La machine virtuelle existante ne prend pas en charge Web Deploy.')
        }
        else
        {
            Write-VerboseWithTime ('{0}: Publishing to VM failed. Attempt {1} of {2}.' -f $scriptName, ($attempts + 1), $allAttempts)
            Write-VerboseWithTime ('{0}: Publishing to VM will start after {1} seconds.' -f $scriptName, $waitingTime)
            
            Start-Sleep -Seconds $waitingTime
        }
                                                                                                                       
         $attempts++
    
         #Réessayez la publication uniquement pour la machine virtuelle qui vient d'être créée et sur laquelle est installé Web Deploy. 
    } While( !$result -and $VMInfo.IsNewCreatedVM -and $attempts -lt $allAttempts -and $Config.cloudService.virtualMachine.enableWebDeployExtension)
    
    if (!$result)
    {                    
        Write-Warning ' Échec de la publication sur la machine virtuelle. Cela peut être dû à un certificat non autorisé ou non valide. Vous pouvez spécifier -AllowUntrusted pour accepter les certificats non autorisés.'
        throw ($scriptName + ' Échec de la publication vers la machine virtuelle.')
    }
}

# Routine principale du script
Set-StrictMode -Version 3
Import-Module Azure

try {
    $AzureToolsUserAgentString = New-Object -TypeName System.Net.Http.Headers.ProductInfoHeaderValue -ArgumentList 'VSAzureTools', '1.5'
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.UserAgents.Add($AzureToolsUserAgentString)
} catch {}

Remove-Module AzureVMPublishModule -ErrorAction SilentlyContinue
$scriptDirectory = Split-Path -Parent $PSCmdlet.MyInvocation.MyCommand.Definition
Import-Module ($scriptDirectory + '\AzureVMPublishModule.psm1') -Scope Local -Verbose:$false

New-Variable -Name VMWebDeployWaitTime -Value 30 -Option Constant -Scope Script 
New-Variable -Name AzureWebAppPublishOutput -Value @() -Scope Global -Force
New-Variable -Name SendHostMessagesToOutput -Value $SendHostMessagesToOutput -Scope Global -Force

try
{
    $originalErrorActionPreference = $Global:ErrorActionPreference
    $originalVerbosePreference = $Global:VerbosePreference
    
    if ($PSBoundParameters['Verbose'])
    {
        $Global:VerbosePreference = 'Continue'
    }
    
    $scriptName = $MyInvocation.MyCommand.Name + ':'
    
    Write-VerboseWithTime ($scriptName + ' Démarrer')
    
    $Global:ErrorActionPreference = 'Stop'
    Write-VerboseWithTime ('{0} $ErrorActionPreference a la valeur {1}' -f $scriptName, $ErrorActionPreference)
    
    Write-Debug ('{0}: $PSCmdlet.ParameterSetName = {1}' -f $scriptName, $PSCmdlet.ParameterSetName)

    # Vérifiez que vous disposez du module Windows Azure, version 0.7.4 ou ultérieure.
	$validAzureModule = Test-AzureModule

    if (-not ($validAzureModule))
    {
         throw 'Impossible de charger Azure PowerShell. Pour installer la version la plus récente, accédez à http://go.microsoft.com/fwlink/?LinkID=320552. Si vous avez déjà installé Azure PowerShell, vous devrez peut-être redémarrer votre ordinateur ou importer manuellement le module.'
    }

    # Enregistrez l'abonnement actif. Il sera restauré à l'état Actif plus tard dans le script
    Backup-Subscription -UserSpecifiedSubscription $SubscriptionName
        
    if ($SubscriptionName)
    {

        # Si vous avez fourni un nom d'abonnement, vérifiez que l'abonnement existe dans votre compte.
        if (!(Get-AzureSubscription -SubscriptionName $SubscriptionName))
        {
            throw ("{0} : impossible de trouver le nom d''abonnement $SubscriptionName" -f $scriptName)

        }

        # Définissez l'abonnement spécifié à l'état actif.
        Select-AzureSubscription -SubscriptionName $SubscriptionName | Out-Null

        Write-VerboseWithTime ('{0} : l''abonnement a la valeur {1}' -f $scriptName, $SubscriptionName)
    }

    $Config = Read-ConfigFile $Configuration -HasWebDeployPackage:([Bool]$WebDeployPackage)

    #Générer et empaqueter votre application web
    New-WebDeployPackage

    #Exécuter un test unitaire sur votre application web
    Test-WebApplication

    #Créer l'environnement Windows Azure décrit dans le fichier de configuration JSON

    $newEnvironmentResult = New-AzureWebApplicationVMEnvironment -Configuration $Config -DatabaseServerPassword $DatabaseServerPassword -VMPassword $VMPassword

    #Déployer le package d'applications web si $WebDeployPackage est spécifié par l'utilisateur 
    if($WebDeployPackage)
    {
        Publish-AzureWebApplicationToVM `
            -Config $Config `
            -ConnectionString $newEnvironmentResult.ConnectionString `
            -WebDeployPackage $WebDeployPackage `
            -VMInfo $newEnvironmentResult.VMInfo
    }
}
finally
{
    $Global:ErrorActionPreference = $originalErrorActionPreference
    $Global:VerbosePreference = $originalVerbosePreference

    # Restaurer l'abonnement actif d'origine à l'état Actif
	if($validAzureModule)
	{
   	    Restore-Subscription
	}   

    Write-Output $Global:AzureWebAppPublishOutput    
    $Global:AzureWebAppPublishOutput = @()
}
