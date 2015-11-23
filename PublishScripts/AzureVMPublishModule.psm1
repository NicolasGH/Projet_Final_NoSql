#  AzureVMPublishModule.psm1 est un module de script Windows PowerShell. Ce module exporte des fonctions Windows PowerShell qui automatisent la gestion du cycle de vie pour les applications web. Vous pouvez utiliser ces fonctions en l'état ou les personnaliser pour votre application et votre environnement de publication.

Set-StrictMode -Version 3

# Variable d'enregistrement de l'abonnement d'origine.
$Script:originalCurrentSubscription = $null

# Variable d'enregistrement du compte de stockage d'origine.
$Script:originalCurrentStorageAccount = $null

# Variable d'enregistrement du compte de stockage de l'abonnement spécifique à l'utilisateur.
$Script:originalStorageAccountOfUserSpecifiedSubscription = $null

# Variable d'enregistrement du nom de l'abonnement.
$Script:userSpecifiedSubscription = $null

# Numéro de port Web Deploy
New-Variable -Name WebDeployPort -Value 8172 -Option Constant

<#
.SYNOPSIS
Indique la date et l'heure avant un message.

.DESCRIPTION
Indique la date et l'heure avant un message. Cette fonction est conçue pour les messages écrits dans les flux Error et Verbose.

.PARAMETER  Message
Spécifie les messages sans la date.

.INPUTS
System.String

.OUTPUTS
System.String

.EXAMPLE
PS C:\> Format-DevTestMessageWithTime -Message "Ajout du fichier $filename à l'annuaire"
2/5/2014 1:03:08 PM - Ajout du fichier $filename à l'annuaire

.LINK
Write-VerboseWithTime

.LINK
Write-ErrorWithTime
#>
function Format-DevTestMessageWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    return ((Get-Date -Format G)  + ' - ' + $Message)
}


<#

.SYNOPSIS
Écrit un message d'erreur précédé de l'heure actuelle.

.DESCRIPTION
Écrit un message d'erreur précédé de l'heure actuelle. Cette fonction appelle la fonction Format-DevTestMessageWithTime pour ajouter l'heure au début du message avant de l'écrire dans le flux Error.

.PARAMETER  Message
Spécifie le message dans l'appel du message d'erreur. Vous pouvez utiliser le pipe de la chaîne de message pour la fonction.

.INPUTS
System.String

.OUTPUTS
Aucune. La fonction écrit dans le flux Error.

.EXAMPLE
PS C:> Write-ErrorWithTime -Message "Failed. Cannot find the file."

Write-Error: 2/6/2014 8:37:29 AM - Failed. Cannot find the file.
 + CategoryInfo     : NotSpecified: (:) [Write-Error], WriteErrorException
 + FullyQualifiedErrorId : Microsoft.PowerShell.Commands.WriteErrorException

.LINK
Write-Error

#>
function Write-ErrorWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    $Message | Format-DevTestMessageWithTime | Write-Error
}


<#
.SYNOPSIS
Écrit un message détaillé précédé de l'heure actuelle.

.DESCRIPTION
Écrit un message détaillé précédé de l'heure actuelle. Dans la mesure où il appelle Write-Verbose, le message ne s'affiche que lorsque le script s'exécute avec le paramètre Verbose ou que la préférence VerbosePreference a la valeur Continue.

.PARAMETER  Message
Spécifie le message dans l'appel du message détaillé. Vous pouvez utiliser le pipe de la chaîne de message pour la fonction.

.INPUTS
System.String

.OUTPUTS
Aucune. La fonction écrit dans le flux Verbose.

.EXAMPLE
PS C:> Write-VerboseWithTime -Message "The operation succeeded."
PS C:>
PS C:\> Write-VerboseWithTime -Message "The operation succeeded." -Verbose
VERBOSE: 1/27/2014 11:02:37 AM - The operation succeeded.

.EXAMPLE
PS C:\ps-test> "The operation succeeded." | Write-VerboseWithTime -Verbose
VERBOSE: 1/27/2014 11:01:38 AM - The operation succeeded.

.LINK
Write-Verbose
#>
function Write-VerboseWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    $Message | Format-DevTestMessageWithTime | Write-Verbose
}


<#
.SYNOPSIS
Écrit un message d'hôte précédé de l'heure actuelle.

.DESCRIPTION
Cette fonction écrit un message au programme hôte (Write-Host) en le faisant précéder de l'heure actuelle. L'effet de l'écriture au programme hôte varie. La plupart des programmes qui hébergent Windows PowerShell écrivent ces messages vers la sortie standard.

.PARAMETER  Message
Spécifie le message de base sans la date. Vous pouvez utiliser le pipe de la chaîne de message pour la fonction.

.INPUTS
System.String

.OUTPUTS
Aucune. La fonction écrit le message au programme hôte.

.EXAMPLE
PS C:> Write-HostWithTime -Message "L''opération a réussi."
1/27/2014 11:02:37 AM - L''opération a réussi.

.LINK
Write-Host
#>
function Write-HostWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )
    
    if ((Get-Variable SendHostMessagesToOutput -Scope Global -ErrorAction SilentlyContinue) -and $Global:SendHostMessagesToOutput)
    {
        if (!(Get-Variable -Scope Global AzureWebAppPublishOutput -ErrorAction SilentlyContinue) -or !$Global:AzureWebAppPublishOutput)
        {
            New-Variable -Name AzureWebAppPublishOutput -Value @() -Scope Global -Force
        }

        $Global:AzureWebAppPublishOutput += $Message | Format-DevTestMessageWithTime
    }
    else 
    {
        $Message | Format-DevTestMessageWithTime | Write-Host
    }
}


<#
.SYNOPSIS
Retourne $true si une propriété ou une méthode est membre de l'objet. Sinon, $false.

.DESCRIPTION
Retourne $true si la propriété ou la méthode est membre de l'objet. Cette fonction retourne $false pour les méthodes statiques de la classe et pour les vues, par exemple PSBase et PSObject.

.PARAMETER  Object
Spécifie l'objet dans le test. Entrez une variable qui contient un objet ou une expression qui retourne un objet. Vous ne pouvez pas spécifier de types, par exemple [DateTime], ou utiliser le pipe d'objets pour cette fonction.

.PARAMETER  Member
Spécifie le nom de la propriété ou de la méthode dans le test. Lors de la spécification d'une méthode, omettez les parenthèses placées à la suite du nom de la méthode.

.INPUTS
Aucune. Cette fonction n'accepte aucune entrée du pipeline.

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Test-Member -Object (Get-Date) -Member DayOfWeek
True

.EXAMPLE
PS C:\> $date = Get-Date
PS C:\> Test-Member -Object $date -Member AddDays
True

.EXAMPLE
PS C:\> [DateTime]::IsLeapYear((Get-Date).Year)
True
PS C:\> Test-Member -Object (Get-Date) -Member IsLeapYear
False

.LINK
Get-Member
#>
function Test-Member
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Object,

        [Parameter(Mandatory = $true)]
        [String]
        $Member
    )

    return $null -ne ($Object | Get-Member -Name $Member)
}


<#
.SYNOPSIS
Retourne $true si le module Windows Azure correspond à la version 0.7.4 ou une version ultérieure. Sinon, $false.

.DESCRIPTION
Test-AzureModuleVersion retourne $true si le module Azure correspond à la version 0.7.4 ou une version ultérieure. Elle retourne $false si le module n'est pas installé ou s'il correspond à une version antérieure. Cette fonction n'a aucun paramètre.

.INPUTS
Aucun

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Get-Module Azure -ListAvailable
PS C:\> #No module
PS C:\> Test-AzureModuleVersion
False

.EXAMPLE
PS C:\> (Get-Module Azure -ListAvailable).Version

Major  Minor  Build  Revision
-----  -----  -----  --------
0      7      4      -1

PS C:\> Test-AzureModuleVersion
True

.LINK
Get-Module

.LINK
PSModuleInfo object (http://msdn.microsoft.com/en-us/library/system.management.automation.psmoduleinfo(v=vs.85).aspx)
#>
function Test-AzureModuleVersion
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [System.Version]
        $Version
    )

    return ($Version.Major -gt 0) -or ($Version.Minor -gt 7) -or ($Version.Minor -eq 7 -and $Version.Build -ge 4)
}


<#
.SYNOPSIS
Retourne $true si le module Windows Azure installé correspond à la version 0.7.4 ou une version ultérieure.

.DESCRIPTION
Test-AzureModule retourne $true si le module Windows Azure installé correspond à la version 0.7.4 ou une version ultérieure. Retourne $false si le module n'est pas installé ou s'il correspond à une version antérieure. Cette fonction n'a aucun paramètre.

.INPUTS
Aucun

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Get-Module Azure -ListAvailable
PS C:\> #No module
PS C:\> Test-AzureModule
False

.EXAMPLE
PS C:\> (Get-Module Azure -ListAvailable).Version

Major  Minor  Build  Revision
-----  -----  -----  --------
    0      7      4      -1

PS C:\> Test-AzureModule
True

.LINK
Get-Module

.LINK
PSModuleInfo object (http://msdn.microsoft.com/en-us/library/system.management.automation.psmoduleinfo(v=vs.85).aspx)
#>
function Test-AzureModule
{
    [CmdletBinding()]

    $module = Get-Module -Name Azure

    if (!$module)
    {
        $module = Get-Module -Name Azure -ListAvailable

        if (!$module -or !(Test-AzureModuleVersion $module.Version))
        {
            return $false;
        }
        else
        {
            $ErrorActionPreference = 'Continue'
            Import-Module -Name Azure -Global -Verbose:$false
            $ErrorActionPreference = 'Stop'

            return $true
        }
    }
    else
    {
        return (Test-AzureModuleVersion $module.Version)
    }
}


<#
.SYNOPSIS
Enregistre l'abonnement Microsoft Azure actif dans la variable $Script:originalSubscription du script.

.DESCRIPTION
La fonction Backup-Subscription enregistre l'abonnement Microsoft Azure actif (Get-AzureSubscription -Current) et son compte de stockage, ainsi que l'abonnement modifié par ce script ($UserSpecifiedSubscription) et son compte de stockage, dans la portée du script. En enregistrant les valeurs, vous pouvez utiliser une fonction telle que Restore-Subscription pour restaurer l'abonnement actif d'origine et son compte de stockage à l'état actif, si ce dernier a changé.

.PARAMETER UserSpecifiedSubscription
Spécifie le nom de l'abonnement dans lequel les ressources doivent être créées et publiées. La fonction enregistre les noms de l'abonnement et de ses comptes de stockage dans la portée du script. Ce paramètre est obligatoire.

.INPUTS
Aucun

.OUTPUTS
Aucun

.EXAMPLE
PS C:\> Backup-Subscription -UserSpecifiedSubscription Contoso
PS C:\>

.EXAMPLE
PS C:\> Backup-Subscription -UserSpecifiedSubscription Contoso -Verbose
VERBOSE: Backup-Subscription: Start
VERBOSE: Backup-Subscription: Original subscription is Microsoft Azure MSDN - Visual Studio Ultimate
VERBOSE: Backup-Subscription: End
#>
function Backup-Subscription
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]
        $UserSpecifiedSubscription
    )

    Write-VerboseWithTime 'Backup-Subscription : début'

    $Script:originalCurrentSubscription = Get-AzureSubscription -Current -ErrorAction SilentlyContinue
    if ($Script:originalCurrentSubscription)
    {
        Write-VerboseWithTime ('Backup-Subscription : l''abonnement d''origine est ' + $Script:originalCurrentSubscription.SubscriptionName)
        $Script:originalCurrentStorageAccount = $Script:originalCurrentSubscription.CurrentStorageAccountName
    }
    
    $Script:userSpecifiedSubscription = $UserSpecifiedSubscription
    if ($Script:userSpecifiedSubscription)
    {        
        $userSubscription = Get-AzureSubscription -SubscriptionName $Script:userSpecifiedSubscription -ErrorAction SilentlyContinue
        if ($userSubscription)
        {
            $Script:originalStorageAccountOfUserSpecifiedSubscription = $userSubscription.CurrentStorageAccountName
        }        
    }

    Write-VerboseWithTime 'Backup-Subscription : fin'
}


<#
.SYNOPSIS
Restaure à l'état "actif" l'abonnement Microsoft Azure enregistré dans la variable $Script:originalSubscription du script.

.DESCRIPTION
La fonction Restore-Subscription rend (de nouveau) actif l'abonnement enregistré dans la variable $Script:originalSubscription. Si l'abonnement d'origine a un compte de stockage, cette fonction rend ce compte de stockage actif pour l'abonnement actif. La fonction restaure l'abonnement uniquement s'il existe une variable $SubscriptionName non Null dans l'environnement. Sinon, son exécution s'arrête. Si $SubscriptionName est rempli, mais que $Script:originalSubscription a la valeur $null, Restore-Subscription utilise l'applet de commande Select-AzureSubscription pour effacer les paramètres actuels et par défaut des abonnements dans Microsoft Azure PowerShell. Cette fonction n'a aucun paramètre, elle n'accepte aucune entrée et ne retourne rien (void). Vous pouvez utiliser -Verbose pour écrire des messages dans le flux Verbose.

.INPUTS
Aucun

.OUTPUTS
Aucun

.EXAMPLE
PS C:\> Restore-Subscription
PS C:\>

.EXAMPLE
PS C:\> Restore-Subscription -Verbose
VERBOSE: Restore-Subscription: Start
VERBOSE: Restore-Subscription: End
#>
function Restore-Subscription
{
    [CmdletBinding()]
    param()

    Write-VerboseWithTime 'Restore-Subscription : début'

    if ($Script:originalCurrentSubscription)
    {
        if ($Script:originalCurrentStorageAccount)
        {
            Set-AzureSubscription `
                -SubscriptionName $Script:originalCurrentSubscription.SubscriptionName `
                -CurrentStorageAccountName $Script:originalCurrentStorageAccount
        }

        Select-AzureSubscription -SubscriptionName $Script:originalCurrentSubscription.SubscriptionName
    }
    else 
    {
        Select-AzureSubscription -NoCurrent
        Select-AzureSubscription -NoDefault
    }
    
    if ($Script:userSpecifiedSubscription -and $Script:originalStorageAccountOfUserSpecifiedSubscription)
    {
        Set-AzureSubscription `
            -SubscriptionName $Script:userSpecifiedSubscription `
            -CurrentStorageAccountName $Script:originalStorageAccountOfUserSpecifiedSubscription
    }

    Write-VerboseWithTime 'Restore-Subscription : fin'
}

<#
.SYNOPSIS
Trouve un compte de stockage Microsoft Azure nommé "devtest*" dans l'abonnement actif.

.DESCRIPTION
La fonction Get-AzureVMStorage retourne le nom du premier compte de stockage avec le modèle de nom "devtest*" (non-respect de la casse) à l'emplacement ou dans le groupe d'affinités spécifié. Si le compte de stockage "devtest*" ne correspond pas à l'emplacement ou au groupe d'affinités, la fonction l'ignore. Vous devez spécifier un emplacement ou un groupe d'affinités.

.PARAMETER  Location
Spécifie l'emplacement du compte de stockage. Les valeurs valides correspondent aux emplacements de Microsoft Azure, par exemple "Ouest des États-Unis". Vous pouvez entrer un emplacement ou un groupe d'affinités, mais pas les deux à la fois.

.PARAMETER  AffinityGroup
Spécifie le groupe d'affinités du compte de stockage. Vous pouvez entrer un emplacement ou un groupe d'affinités, mais pas les deux à la fois.

.INPUTS
Aucune. Vous ne pouvez pas utiliser le pipe d'entrée pour cette fonction.

.OUTPUTS
System.String

.EXAMPLE
PS C:\> Get-AzureVMStorage -Location "East US"
devtest3-fabricam

.EXAMPLE
PS C:\> Get-AzureVMStorage -AffinityGroup Finance
PS C:\>

.EXAMPLE\
PS C:\> Get-AzureVMStorage -AffinityGroup Finance -Verbose
VERBOSE: Get-AzureVMStorage: Start
VERBOSE: Get-AzureVMStorage: End

.LINK
Get-AzureStorageAccount
#>
function Get-AzureVMStorage
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ParameterSetName = 'Location')]
        [String]
        $Location,

        [Parameter(Mandatory = $true, ParameterSetName = 'AffinityGroup')]
        [String]
        $AffinityGroup
    )

    Write-VerboseWithTime 'Get-AzureVMStorage : début'

    $storages = @(Get-AzureStorageAccount -ErrorAction SilentlyContinue)
    $storageName = $null

    foreach ($storage in $storages)
    {
        # Obtenir le premier compte de stockage dont le nom commence par "devtest"
        if ($storage.Label -like 'devtest*')
        {
            if ($storage.AffinityGroup -eq $AffinityGroup -or $storage.Location -eq $Location)
            {
                $storageName = $storage.Label

                    Write-HostWithTime ('Get-AzureVMStorage : compte de stockage devtest trouvé ' + $storageName)
                    $storage | Out-String | Write-VerboseWithTime
                break
            }
        }
    }

    Write-VerboseWithTime 'Get-AzureVMStorage: fin'
    return $storageName
}


<#
.SYNOPSIS
Crée un compte de stockage Microsoft Azure dont le nom unique commence par "devtest".

.DESCRIPTION
La fonction Add-AzureVMStorage crée un compte de stockage Microsoft Azure dans l'abonnement actif. Le nom du compte commence par "devtest" suivi d'une chaîne alphanumérique unique. La fonction retourne le nom du nouveau compte de stockage. Vous devez spécifier un emplacement ou un groupe d'affinités pour le nouveau compte de stockage.

.PARAMETER  Location
Spécifie l'emplacement du compte de stockage. Les valeurs valides correspondent aux emplacements de Microsoft Azure, par exemple "Ouest des États-Unis". Vous pouvez entrer un emplacement ou un groupe d'affinités, mais pas les deux à la fois.

.PARAMETER  AffinityGroup
Spécifie le groupe d'affinités du compte de stockage. Vous pouvez entrer un emplacement ou un groupe d'affinités, mais pas les deux à la fois.

.INPUTS
Aucune. Vous ne pouvez pas utiliser le pipe d'entrée pour cette fonction.

.OUTPUTS
System.String. La chaîne correspond au nom du nouveau compte de stockage

.EXAMPLE
PS C:\> Add-AzureVMStorage -Location "East Asia"
devtestd6b45e23a6dd4bdab

.EXAMPLE
PS C:\> Add-AzureVMStorage -AffinityGroup Finance
devtestd6b45e23a6dd4bdab

.EXAMPLE
PS C:\> Add-AzureVMStorage -AffinityGroup Finance -Verbose
VERBOSE: Add-AzureVMStorage: Start
VERBOSE: Add-AzureVMStorage: Created new storage acccount devtestd6b45e23a6dd4bdab"
VERBOSE: Add-AzureVMStorage: End
devtestd6b45e23a6dd4bdab

.LINK
New-AzureStorageAccount
#>
function Add-AzureVMStorage
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ParameterSetName = 'Location')]
        [String]
        $Location,

        [Parameter(Mandatory = $true, ParameterSetName = 'AffinityGroup')]
        [String]
        $AffinityGroup
    )

    Write-VerboseWithTime 'Add-AzureVMStorage : début'

    # Créer un nom unique en ajoutant une partie d'un GUID à "devtest"
    $name = 'devtest'
    $suffix = [guid]::NewGuid().ToString('N').Substring(0,24 - $name.Length)
    $name = $name + $suffix

    # Créer un compte de stockage Microsoft Azure avec l'emplacement/le groupe d'affinités
    if ($PSCmdlet.ParameterSetName -eq 'Location')
    {
        New-AzureStorageAccount -StorageAccountName $name -Location $Location | Out-Null
    }
    else
    {
        New-AzureStorageAccount -StorageAccountName $name -AffinityGroup $AffinityGroup | Out-Null
    }

    Write-HostWithTime ("Add-AzureVMStorage : compte de stockage $name créé")
    Write-VerboseWithTime 'Add-AzureVMStorage : fin'
    return $name
}


<#
.SYNOPSIS
Valide le fichier de configuration et retourne une table de hachage des valeurs du fichier de configuration.

.DESCRIPTION
La fonction Read-ConfigFile valide le fichier de configuration JSON et retourne une table de hachage des valeurs sélectionnées.
-- Elle commence par convertir le fichier JSON en PSCustomObject.
La table de hachage pour service cloud comprend les clés suivantes :
-- webdeployparameters : Facultatif. Peut avoir la valeur <$null ou être vide.
-- Databases: Bases de données SQL

.PARAMETER  ConfigurationFile
Spécifie le chemin d'accès et le nom du fichier de configuration JSON de votre projet web. Visual Studio génère le fichier JSON automatiquement lorsque vous créez un projet web et que vous le stockez dans le dossier PublishScripts de votre solution.

.PARAMETER HasWebDeployPackage
Indique la présence d'un fichier ZIP de package Web Deploy pour l'application web. Pour spécifier une valeur de $true, utilisez -HasWebDeployPackage ou HasWebDeployPackage:$true. Pour spécifier une valeur de false, utilisez HasWebDeployPackage:$false. Ce paramètre est obligatoire.

.INPUTS
Aucune. Vous ne pouvez pas utiliser le pipe d'entrée pour cette fonction.

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
PS C:\> Read-ConfigFile -ConfigurationFile <path> -HasWebDeployPackage


Name                           Value                                                                                                                                                                     
----                           -----                                                                                                                                                                     
databases                      {@{connectionStringName=; databaseName=; serverName=; user=; password=}}                                                                                                  
cloudService                   @{name="contoso"; affinityGroup="contosoEast"; location=; virtualNetwork=; subnet=; availabilitySet=; virtualMachine=}                                                      
webDeployParameters            @{iisWebApplicationName="Default Web Site"} 
#>
function Read-ConfigFile
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $ConfigurationFile,

        [Parameter(Mandatory = $true)]
        [Switch]
        $HasWebDeployPackage	    
    )

    Write-VerboseWithTime 'Read-ConfigFile : début'

    # Obtenir le contenu du fichier JSON (-raw ignore les sauts de ligne) et le convertir en PSCustomObject
    $config = Get-Content $ConfigurationFile -Raw | ConvertFrom-Json

    if (!$config)
    {
        throw ('Échec de Read-ConfigFile : ConvertFrom-Json : ' + $error[0])
    }

    # Déterminer si l'objet environmentSettings a les propriétés 'cloudService' (quelle que soit la valeur de la propriété)
    $hasCloudServiceProperty = Test-Member -Object $config.environmentSettings -Member 'cloudService'

    if (!$hasCloudServiceProperty)
    {
        throw 'Read-ConfigFile : le fichier de configuration ne contient pas de propriété cloudService.'
    }

    # Créer une table de hachage à partir des valeurs de PSCustomObject
    $returnObject = New-Object -TypeName Hashtable

        $returnObject.Add('cloudService', $config.environmentSettings.cloudService)
        if ($HasWebDeployPackage)
        {
            $returnObject.Add('webDeployParameters', $config.environmentSettings.webdeployParameters)
        }

    if (Test-Member -Object $config.environmentSettings -Member 'databases')
    {
        $returnObject.Add('databases', $config.environmentSettings.databases)
    }

    Write-VerboseWithTime 'Read-ConfigFile : fin'

    return $returnObject
}

<#
.SYNOPSIS
Ajoute de nouveaux points de terminaison d'entrée à une machine virtuelle et retourne la machine virtuelle avec le nouveau point de terminaison.

.DESCRIPTION
La fonction Add-AzureVMEndpoints ajoute de nouveaux points de terminaison d'entrée à une machine virtuelle et retourne la machine virtuelle avec les nouveaux points de terminaison. Cette fonction appelle l'applet de commande Add-AzureEndpoint (module Windows Azure).

.PARAMETER  VM
Spécifie l'objet de machine virtuelle. Entrez un objet de machine virtuelle, par exemple le type retourné par les applets de commande New-AzureVM ou Get-AzureVM. Vous pouvez utiliser le pipe d'objets à partir de Get-AzureVM pour Add-AzureVMEndpoints.

.PARAMETER  Endpoints
Spécifie un tableau de points de terminaison à ajouter à la machine virtuelle. En règle générale, ces points de terminaison proviennent du fichier de configuration JSON généré par Visual Studio pour les projets web. Utilisez la fonction Read-ConfigFile de ce module pour convertir le fichier en table de hachage. Les points de terminaison correspondent à une propriété de la clé cloudservice de la table de hachage ($<table_hachage>.cloudservice.virtualmachine.endpoints). Par exemple,
PS C:\> $config.cloudservice.virtualmachine.endpoints
name      protocol publicport privateport
----      -------- ---------- -----------
http      tcp      80         80
https     tcp      443        443
WebDeploy tcp      8172       8172

.INPUTS
Microsoft.WindowsAzure.Commands.ServiceManagement.Model.IPersistentVM

.OUTPUTS
Microsoft.WindowsAzure.Commands.ServiceManagement.Model.IPersistentVM

.EXAMPLE
Get-AzureVM

.EXAMPLE

.LINK
Get-AzureVM

.LINK
Add-AzureEndpoint
#>
function Add-AzureVMEndpoints
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.WindowsAzure.Commands.ServiceManagement.Model.PersistentVM]
        $VM,

        [Parameter(Mandatory = $true)]
        [PSCustomObject[]]
        $Endpoints
    )

    Write-VerboseWithTime 'Add-AzureVMEndpoints : début'

    # Ajouter chaque point de terminaison du fichier JSON à la machine virtuelle
    $Endpoints | ForEach-Object `
    {
        $_ | Out-String | Write-VerboseWithTime
        Add-AzureEndpoint -VM $VM -Name $_.name -Protocol $_.protocol -LocalPort $_.privateport -PublicPort $_.publicport | Out-Null
    }

    Write-VerboseWithTime 'Add-AzureVMEndpoints : fin'
    return $VM
}

<#
.SYNOPSIS
Crée tous les éléments d'une nouvelle machine virtuelle dans un abonnement Microsoft Azure.

.DESCRIPTION
Cette fonction crée une machine virtuelle Microsoft Azure et retourne l'URL de la machine virtuelle déployée. La fonction définit les conditions requises, puis appelle l'applet de commande New-AzureVM (module Microsoft Azure) pour créer une machine virtuelle. 
-- Elle appelle l'applet de commande New-AzureVMConfig (module Windows Azure) pour obtenir un objet de configuration de machine virtuelle. 
-- Si vous incluez le paramètre Subnet pour ajouter la machine virtuelle à un sous-réseau Windows Azure, elle appelle Set-AzureSubnet pour définir la liste des sous-réseaux de la machine virtuelle. 
-- Elle appelle Add-AzureProvisioningConfig (module Windows Azure) pour ajouter des éléments à la configuration de la machine virtuelle. Elle crée une configuration d'approvisionnement Windows autonome (-Windows) avec un compte d'administrateur et un mot de passe. 
-- Elle appelle la fonction Add-AzureVMEndpoints dans ce module pour ajouter les points de terminaison spécifiés par le paramètre Endpoints. Cette fonction prend un objet de machine virtuelle et retourne un objet de machine virtuelle avec les points de terminaison ajoutés. 
-- Elle appelle l'applet de commande Add-AzureVM pour créer une machine virtuelle Microsoft Azure, puis retourne la nouvelle machine virtuelle. Les valeurs des paramètres de la fonction proviennent généralement du fichier de configuration JSON généré par Visual Studio pour les projets web intégrés à Microsoft Azure. La fonction Read-ConfigFile de ce module convertit le fichier JSON en table de hachage. Enregistre la clé cloudservice de la table de hachage dans une variable (en tant que PSCustomObject), puis utilise les propriétés de l'objet personnalisé en tant que valeurs de paramètre.

.PARAMETER  VMName
Spécifie un nom pour la nouvelle machine virtuelle. Le nom de la machine virtuelle doit être unique dans le service cloud. Ce paramètre est obligatoire.

.PARAMETER  VMSize
Spécifie la taille de la machine virtuelle. Les valeurs valides sont "ExtraSmall", "Small", "Medium", "Large", "ExtraLarge", "A5", "A6" et "A7". Cette valeur est soumise en tant que valeur du paramètre InstanceSize de New-AzureVMConfig. Ce paramètre est obligatoire. 

.PARAMETER  ServiceName
Spécifie un service Microsoft Azure existant ou le nom d'un nouveau service Microsoft Azure. Cette valeur est soumise au paramètre ServiceName de l'applet de commande New-AzureVM, qui ajoute la nouvelle machine virtuelle à un service Microsoft Azure existant ou (si Location ou AffinityGroup est spécifié) qui crée une machine virtuelle et un service dans l'abonnement actif. Ce paramètre est obligatoire. 

.PARAMETER  ImageName
Spécifie le nom de l'image de machine virtuelle à utiliser pour le disque du système d'exploitation. Ce paramètre est soumis en tant que valeur du paramètre ImageName de l'applet de commande New-AzureVMConfig. Ce paramètre est obligatoire. 

.PARAMETER  UserName
Spécifie un nom d'administrateur. Il est soumis en tant que valeur du paramètre AdminUserName de Add-AzureProvisioningConfig. Ce paramètre est obligatoire.

.PARAMETER  UserPassword
Spécifie un mot de passe pour le compte d'administrateur. Il est soumis en tant que valeur du paramètre Password de Add-AzureProvisioningConfig. Ce paramètre est obligatoire.

.PARAMETER  Endpoints
Spécifie un tableau de points de terminaison à ajouter à la machine virtuelle. Cette valeur est soumise à la fonction Add-AzureVMEndpoints exportée par ce module. Ce paramètre est facultatif. En règle générale, ces points de terminaison proviennent du fichier de configuration JSON généré par Visual Studio pour les projets web. Utilisez la fonction Read-ConfigFile de ce module pour convertir le fichier en table de hachage. Les points de terminaison correspondent à une propriété de la clé cloudService de la table de hachage ($<table_hachage>.cloudservice.virtualmachine.endpoints). 

.PARAMETER  AvailabilitySetName
Spécifie le nom d'un groupe à haute disponibilité pour la nouvelle machine virtuelle. Lorsque vous placez plusieurs machines virtuelles dans un groupe à haute disponibilité, Microsoft Azure essaie de conserver les machines virtuelles sur des hôtes distincts afin d'améliorer la continuité du service en cas de défaillance de l'une d'entre elles. Ce paramètre est facultatif. 

.PARAMETER  VNetName
Spécifie le nom du réseau virtuel où la nouvelle machine virtuelle est déployée. Cette valeur est soumise au paramètre VNetName de l'applet de commande Add-AzureVM. Ce paramètre est facultatif. 

.PARAMETER  Location
Spécifie un emplacement pour la nouvelle machine virtuelle. Les valeurs valides correspondent aux emplacements de Microsoft Azure, par exemple "Ouest des États-Unis". La valeur par défaut correspond à l'emplacement de l'abonnement. Ce paramètre est facultatif. 

.PARAMETER  AffinityGroup
Spécifie un groupe d'affinités pour la nouvelle machine virtuelle. Un groupe d'affinités est un groupe de ressources associées. Lorsque vous spécifiez un groupe d'affinités, Microsoft Azure essaie de maintenir ensemble les ressources du groupe pour optimiser l'efficacité. 

.PARAMETER  Subnet
Spécifie le sous-réseau de la nouvelle configuration de machine virtuelle. Cette valeur est soumise à l'applet de commande Set-AzureSubnet (module Windows Azure) qui prend une machine virtuelle et un ensemble de noms de sous-réseaux, puis retourne une machine virtuelle avec les sous-réseaux contenus dans sa configuration.

.PARAMETER EnableWebDeployExtension
Prépare la machine virtuelle en vue de son déploiement. Prépare la machine virtuelle en vue de son déploiement. Ce paramètre est facultatif. S'il n'est pas spécifié, la machine virtuelle est créée mais n'est pas déployée. La valeur de ce paramètre est incluse dans le fichier de configuration JSON généré par Visual Studio pour les services cloud.

.PARAMETER VMImage
Indique que le ImageName est le nom d'un VMImage et non d'un OSImage. Ce paramètre est facultatif. S'il n'est pas spécifié, ImageName sera traité comme un OSImage. La valeur de ce paramètre est incluse dans le fichier de configuration JSON que Visual Studio génère pour ces machines virtuelles.

.PARAMETER GeneralizedImage
Pour un VMImage, spécifie si l'état du système d'exploitation est généralisé. Ce paramètre est facultatif. S'il n'est pas spécifié, le script se comporte comme pour un VMImage spécialisé. Ce paramètre est ignoré pour OSImages. La valeur de ce paramètre est incluse dans le fichier de configuration JSON que Visual génère pour les machines virtuelles.

.INPUTS
Aucune. Cette fonction n'accepte aucune entrée du pipeline.

.OUTPUTS
System.Url

.EXAMPLE
 Cette commande appelle la fonction Add-AzureVM. Un grand nombre des valeurs de paramètre proviennent d'un objet $CloudServiceConfiguration. PSCustomObject correspond à la clé cloudservice et aux valeurs de la table de hachage retournée par la fonction Read-ConfigFile. La source correspond au fichier de configuration JSON généré par Visual Studio pour les projets web.

PS C:\> $config = Read-Configfile <name>.json
PS C:\> $CloudServiceConfiguration = $config.cloudservice

PS C:\> Add-AzureVM `
-UserName $userName `
-UserPassword  $userPassword `
-ImageName $CloudServiceConfiguration.virtualmachine.vhdImage `
-VMName $CloudServiceConfiguration.virtualmachine.name `
-VMSize $CloudServiceConfiguration.virtualmachine.size`
-Endpoints $CloudServiceConfiguration.virtualmachine.endpoints `
-ServiceName $serviceName `
-Location $CloudServiceConfiguration.location `
-AvailabilitySetName $CloudServiceConfiguration.availabilitySet `
-VNetName $CloudServiceConfiguration.virtualNetwork `
-Subnet $CloudServiceConfiguration.subnet `
-AffinityGroup $CloudServiceConfiguration.affinityGroup `
-EnableWebDeployExtension

http://contoso.cloudapp.net

.EXAMPLE
PS C:\> $endpoints = [PSCustomObject]@{name="http";protocol="tcp";publicport=80;privateport=80}, `
                        [PSCustomObject]@{name="https";protocol="tcp";publicport=443;privateport=443},`
                        [PSCustomObject]@{name="WebDeploy";protocol="tcp";publicport=8172;privateport=8172}
PS C:\> Add-AzureVM `
-UserName admin01 `
-UserPassword "password" `
-ImageName bd507d3a70934695bc2128e3e5a255ba__RightImage-Windows-2012-x64-v13.4.12.2 `
-VMName DevTestVM123 `
-VMSize Small `
-Endpoints $endpoints `
-ServiceName DevTestVM1234 `
-Location "West US"

.LINK
New-AzureVMConfig

.LINK
Set-AzureSubnet

.LINK
Add-AzureProvisioningConfig

.LINK
Get-AzureDeployment
#>
function Add-AzureVM
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $VMName,

        [Parameter(Mandatory = $true)]
        [String]
        $VMSize,

        [Parameter(Mandatory = $true)]
        [String]
        $ServiceName,

        [Parameter(Mandatory = $true)]
        [String]
        $ImageName,

        [Parameter(Mandatory = $false)]
        [String]
        $UserName,

        [Parameter(Mandatory = $false)]
        [String]
        $UserPassword,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Object[]]
        $Endpoints,

        [Parameter(Mandatory = $false)]
        [AllowEmptyString()]
        [String]
        $AvailabilitySetName,

        [Parameter(Mandatory = $false)]
        [AllowEmptyString()]
        [String]
        $VNetName,

        [Parameter(Mandatory = $false)]
        [AllowEmptyString()]
        [String]
        $Location,

        [Parameter(Mandatory = $false)]
        [AllowEmptyString()]
        [String]
        $AffinityGroup,

        [Parameter(Mandatory = $false)]
        [AllowEmptyString()]
        [String]
        $Subnet,

        [Parameter(Mandatory = $false)]
        [Switch]
        $EnableWebDeployExtension,

        [Parameter(Mandatory=$false)]
        [Switch]
        $VMImage,

        [Parameter(Mandatory=$false)]
        [Switch]
        $GeneralizedImage
    )

    Write-VerboseWithTime 'Add-AzureVM : début'

	if ($VMImage)
	{
		$specializedImage = !$GeneralizedImage;
	}
	else
	{
		$specializedImage = $false;
	}

    # Créez un objet de configuration de machine virtuelle de Microsoft Azure.
    if ($AvailabilitySetName)
    {
        $vm = New-AzureVMConfig -Name $VMName -InstanceSize $VMSize -ImageName $ImageName -AvailabilitySetName $AvailabilitySetName
    }
    else
    {
        $vm = New-AzureVMConfig -Name $VMName -InstanceSize $VMSize -ImageName $ImageName
    }

    if (!$vm)
    {
        throw 'Add-AzureVM : échec de la création de la configuration d''une machine virtuelle Windows Azure.'
    }

    if ($Subnet)
    {
        # Définissez la liste des sous-réseaux pour une configuration de machine virtuelle.
        $subnetResult = Set-AzureSubnet -VM $vm -SubnetNames $Subnet

        if (!$subnetResult)
        {
            throw ('Add-AzureVM : échec de la définition du sous-réseau ' + $Subnet)
        }
    }

    if (!$specializedImage)
    {
	    # Ajouter des données de configuration à la configuration de la machine virtuelle
        $vm = Add-AzureProvisioningConfig -VM $vm -Windows -Password $UserPassword -AdminUserName $UserName -NoRDPEndpoint -NoWinRMEndpoint

        if (!$vm)
		{
			throw ('Add-AzureVM : échec de la création de la configuration de l''approvisionnement.')
		}
    }

    # Ajouter des points de terminaison d'entrée à la machine virtuelle
    if ($Endpoints -and $Endpoints.Count -gt 0)
    {
        $vm = Add-AzureVMEndpoints -Endpoints $Endpoints -VM $vm
    }

    if (!$vm)
    {
        throw ('Add-AzureVM : échec de la création des points de terminaison.')
    }

    if ($EnableWebDeployExtension)
    {
        Write-VerboseWithTime 'Add-AzureVM : ajouter l''extension webdeploy'

        Write-VerboseWithTime 'Pour voir les termes du contrat de licence relatif à WebDeploy, consultez http://go.microsoft.com/fwlink/?LinkID=389744 '

        $vm = Set-AzureVMExtension `
            -VM $vm `
            -ExtensionName WebDeployForVSDevTest `
            -Publisher 'Microsoft.VisualStudio.WindowsAzure.DevTest' `
            -Version '1.*' 

        if (!$vm)
        {
            throw ('Add-AzureVM : échec de l''ajout de l''extension webdeploy.')
        }
    }

    # Créer une table de hachage des paramètres pour la projection
    $param = New-Object -TypeName Hashtable
    if ($VNetName)
    {
        $param.Add('VNetName', $VNetName)
    }

    # VMImages ne prend pas en charge l'emplacement actuellement - la nouvelle machine virtuelle sera créée dans le compte de stockage (emplacement) dans lequel se trouve l'image
    if (!$VMImage -and $Location)
    {
		$param.Add('Location', $Location)
    }

    if ($AffinityGroup)
    {
        $param.Add('AffinityGroup', $AffinityGroup)
    }

    $param.Add('ServiceName', $ServiceName)
    $param.Add('VMs', $vm)
    $param.Add('WaitForBoot', $true)

    $param | Out-String | Write-VerboseWithTime

    New-AzureVM @param | Out-Null

    Write-HostWithTime ('Add-AzureVM : machine virtuelle créée ' + $VMName)

    $url = [System.Uri](Get-AzureDeployment -ServiceName $ServiceName).Url

    if (!$url)
    {
        throw 'Add-AzureVM : impossible de trouver l''URL de la machine virtuelle.'
    }

    Write-HostWithTime ('Add-AzureVM : publier l''URL https://' + $url.Host + ':' + $WebDeployPort + '/msdeploy.axd')

    Write-VerboseWithTime 'Add-AzureVM : fin'

    return $url.AbsoluteUri
}


<#
.SYNOPSIS
Obtient la machine virtuelle Microsoft Azure spécifiée.

.DESCRIPTION
La fonction Find-AzureVM obtient une machine virtuelle Microsoft Azure en fonction du nom du service et du nom de la machine virtuelle. Cette fonction appelle l'applet de commande Test-AzureName (module Azure) pour vérifier que le nom du service existe dans Microsoft Azure. Si c'est le cas, la fonction appelle l'applet de commande Get-AzureVM pour obtenir la machine virtuelle. Cette fonction retourne une table de hachage avec les clés vm et foundService.
-- FoundService: $True Si Test-AzureName a trouvé le service. Sinon, $False
-- VM: Contient l'objet de machine virtuelle quand FoundService a la valeur true et que Get-AzureVM retourne l'objet de machine virtuelle.

.PARAMETER  ServiceName
Nom d'un service Microsoft Azure existant. Ce paramètre est obligatoire.

.PARAMETER  VMName
Nom d'une machine virtuelle dans le service. Ce paramètre est obligatoire.

.INPUTS
Aucune. Vous ne pouvez pas utiliser le pipe d'entrée pour cette fonction.

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
PS C:\> Find-AzureVM -Service Contoso -Name ContosoVM2

Name                           Value
----                           -----
foundService                   True

DeploymentName        : Contoso
Name                  : ContosoVM2
Label                 :
VM                    : Microsoft.WindowsAzure.Commands.ServiceManagement.Model.PersistentVM
InstanceStatus        : ReadyRole
IpAddress             : 100.71.114.118
InstanceStateDetails  :
PowerState            : Started
InstanceErrorCode     :
InstanceFaultDomain   : 0
InstanceName          : ContosoVM2
InstanceUpgradeDomain : 0
InstanceSize          : Small
AvailabilitySetName   :
DNSName               : http://contoso.cloudapp.net/
ServiceName           : Contoso
OperationDescription  : Get-AzureVM
OperationId           : 3c38e933-9464-6876-aaaa-734990a882d6
OperationStatus       : Succeeded

.LINK
Get-AzureVM
#>
function Find-AzureVM
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $ServiceName,

        [Parameter(Mandatory = $true)]
        [String]
        $VMName
    )

    Write-VerboseWithTime 'Find-AzureVM : début'
    $foundService = $false
    $vm = $null

    if (Test-AzureName -Service -Name $ServiceName)
    {
        $foundService = $true
        $vm = Get-AzureVM -ServiceName $ServiceName -Name $VMName
        if ($vm)
        {
            Write-HostWithTime ('Find-AzureVM : machine virtuelle existante trouvée ' + $vm.Name )
            $vm | Out-String | Write-VerboseWithTime
        }
    }

    Write-VerboseWithTime 'Find-AzureVM : fin'
    return @{ VM = $vm; FoundService = $foundService }
}


<#
.SYNOPSIS
Trouve ou crée une machine virtuelle dans l'abonnement qui correspond aux valeurs du fichier de configuration JSON.

.DESCRIPTION
La fonction New-AzureVMEnvironment trouve ou crée une machine virtuelle dans l'abonnement qui correspond aux valeurs du fichier de configuration JSON généré par Visual Studio pour les projets web. Elle accepte PSCustomObject, qui représente la clé cloudservice de la table de hachage retournée par Read-ConfigFile. Ces données proviennent du fichier de configuration JSON généré par Visual Studio. La fonction recherche une machine virtuelle dans l'abonnement dont le nom de service et le nom de machine virtuelle correspondent aux valeurs de l'objet personnalisé CloudServiceConfiguration. Si elle ne peut pas trouver de machine virtuelle correspondante, elle appelle la fonction Add-AzureVM de ce module et utilise les valeurs de l'objet CloudServiceConfiguration pour créer une machine virtuelle. L'environnement de la machine virtuelle comprend un compte de stockage dont le nom commence par "devtest". Si la fonction ne peut pas trouver de compte de stockage avec ce modèle de nom dans l'abonnement, elle en crée un. La fonction retourne une table de hachage avec les clés VmUrl, userName et Password, ainsi que les valeurs de chaîne.

.PARAMETER  CloudServiceConfiguration
Prend PSCustomObject, qui contient la propriété cloudservice de la table de hachage retournée par la fonction Read-ConfigFile. Toutes les valeurs proviennent du fichier de configuration JSON généré par Visual Studio pour les projets web. Vous pouvez trouver ce fichier dans le dossier PublishScripts de votre solution. Ce paramètre est obligatoire.
$config = Read-ConfigFile -ConfigurationFile <file>.json $cloudServiceConfiguration = $config.cloudService

.PARAMETER  VMPassword
Utilise une table de hachage avec des clés de nom et de mot de passe, telles que : @{Name = "admin"; Password = "password"}. Ce paramètre est facultatif. Si vous l'omettez, les valeurs par défaut sont le nom d'utilisateur et le mot de passe de la machine virtuelle dans le fichier de configuration JSON.

.INPUTS
PSCustomObject  System.Collections.Hashtable

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
$config = Read-ConfigFile -ConfigurationFile $<file>.json
$cloudSvcConfig = $config.cloudService
$namehash = @{name = "admin"; password = "password"}

New-AzureVMEnvironment `
    -CloudServiceConfiguration $cloudSvcConfig `
    -VMPassword $namehash

Name                           Value
----                           -----
UserName                       admin
VMUrl                          contoso.cloudnet.net
Password                       password

.LINK
Add-AzureVM

.LINK
New-AzureStorageAccount
#>
function New-AzureVMEnvironment
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $CloudServiceConfiguration,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $VMPassword
    )

    Write-VerboseWithTime ('New-AzureVMEnvironment : début')

    if ($CloudServiceConfiguration.location -and $CloudServiceConfiguration.affinityGroup)
    {
        throw 'New-AzureVMEnvironment : fichier de configuration incorrect. A location et affinityGroup'
    }

    if (!$CloudServiceConfiguration.location -and !$CloudServiceConfiguration.affinityGroup)
    {
        throw 'New-AzureVMEnvironment : fichier de configuration incorrect. N''a pas location ou affinityGroup'
    }

    # Si l'objet CloudServiceConfiguration a la propriété 'name' (pour le nom de service) et si la propriété 'name' a une valeur, servez-vous en. Sinon, utilisez le nom de la machine virtuelle dans l'objet CloudServiceConfiguration, qui est toujours rempli.
    if ((Test-Member $CloudServiceConfiguration 'name') -and $CloudServiceConfiguration.name)
    {
        $serviceName = $CloudServiceConfiguration.name
    }
    else
    {
        $serviceName = $CloudServiceConfiguration.virtualMachine.name
    }

    if (!$VMPassword)
    {
        $userName = $CloudServiceConfiguration.virtualMachine.user
        $userPassword = $CloudServiceConfiguration.virtualMachine.password
    }
    else
    {
        $userName = $VMPassword.Name
        $userPassword = $VMPassword.Password
    }

    # Obtenir le nom de la machine virtuelle à partir du fichier JSON
    $findAzureVMResult = Find-AzureVM -ServiceName $serviceName -VMName $CloudServiceConfiguration.virtualMachine.name

    # Si nous ne pouvons pas trouver de machine virtuelle portant ce nom dans ce service cloud, créez-en un.
    if (!$findAzureVMResult.VM)
    {
        if(!$CloudServiceConfiguration.virtualMachine.isVMImage)
        {
            $storageAccountName = $null
            $imageInfo = Get-AzureVMImage -ImageName $CloudServiceConfiguration.virtualmachine.vhdimage 
            if ($imageInfo -and $imageInfo.Category -eq 'User')
            {
                $storageAccountName = ($imageInfo.MediaLink.Host -split '\.')[0]
            }

            if (!$storageAccountName)
            {
                if ($CloudServiceConfiguration.location)
                {
                    $storageAccountName = Get-AzureVMStorage -Location $CloudServiceConfiguration.location
                }
                else
                {
                    $storageAccountName = Get-AzureVMStorage -AffinityGroup $CloudServiceConfiguration.affinityGroup
                }
            }

             # S'il n'existe pas de compte de stockage devtest*, créez-en un.
            if (!$storageAccountName)
            {
                if ($CloudServiceConfiguration.location)
                {
                    $storageAccountName = Add-AzureVMStorage -Location $CloudServiceConfiguration.location
                }
                else
                {
                    $storageAccountName = Add-AzureVMStorage -AffinityGroup $CloudServiceConfiguration.affinityGroup
                }
            }

            $currentSubscription = Get-AzureSubscription -Current

            if (!$currentSubscription)
            {
                throw 'New-AzureVMEnvironment : échec de l''obtention de l''abonnement Windows Azure actif.'
            }

            # Définir le compte de stockage devtest* comme actif
            Set-AzureSubscription `
                -SubscriptionName $currentSubscription.SubscriptionName `
                -CurrentStorageAccountName $storageAccountName

            Write-VerboseWithTime ('New-AzureVMEnvironment : le compte de stockage a la valeur ' + $storageAccountName)
        }

        $location = ''            
        if (!$findAzureVMResult.FoundService)
        {
            $location = $CloudServiceConfiguration.location
        }

        $endpoints = $null
        if (Test-Member -Object $CloudServiceConfiguration.virtualmachine -Member 'Endpoints')
        {
            $endpoints = $CloudServiceConfiguration.virtualmachine.endpoints
        }

        # Créer une machine virtuelle avec les valeurs du fichier JSON + les valeurs de paramètre
        $VMUrl = Add-AzureVM `
            -UserName $userName `
            -UserPassword $userPassword `
            -ImageName $CloudServiceConfiguration.virtualMachine.vhdImage `
            -VMName $CloudServiceConfiguration.virtualMachine.name `
            -VMSize $CloudServiceConfiguration.virtualMachine.size`
            -Endpoints $endpoints `
            -ServiceName $serviceName `
            -Location $location `
            -AvailabilitySetName $CloudServiceConfiguration.availabilitySet `
            -VNetName $CloudServiceConfiguration.virtualNetwork `
            -Subnet $CloudServiceConfiguration.subnet `
            -AffinityGroup $CloudServiceConfiguration.affinityGroup `
            -EnableWebDeployExtension:$CloudServiceConfiguration.virtualMachine.enableWebDeployExtension `
            -VMImage:$CloudServiceConfiguration.virtualMachine.isVMImage `
            -GeneralizedImage:$CloudServiceConfiguration.virtualMachine.isGeneralizedImage

        Write-VerboseWithTime ('New-AzureVMEnvironment : fin')

        return @{ 
            VMUrl = $VMUrl; 
            UserName = $userName; 
            Password = $userPassword; 
            IsNewCreatedVM = $true; }
    }
    else
    {
        Write-VerboseWithTime ('New-AzureVMEnvironment : machine virtuelle existante trouvée ' + $findAzureVMResult.VM.Name)
    }

    Write-VerboseWithTime ('New-AzureVMEnvironment : fin')

    return @{ 
        VMUrl = $findAzureVMResult.VM.DNSName; 
        UserName = $userName; 
        Password = $userPassword; 
        IsNewCreatedVM = $false; }
}


<#
.SYNOPSIS
Retourne une commande permettant d'exécuter l'outil MsDeploy.exe

.DESCRIPTION
La fonction Get-MSDeployCmd assemble et retourne une commande valide pour exécuter l'outil Web Deploy, MSDeploy.exe. Elle trouve le chemin d'accès approprié de l'outil sur l'ordinateur local dans une clé de Registre. Cette fonction n'a aucun paramètre.

.INPUTS
Aucun

.OUTPUTS
System.String

.EXAMPLE
PS C:\> Get-MSDeployCmd
C:\Program Files\IIS\Microsoft Web Deploy V3\MsDeploy.exe

.LINK
Get-MSDeployCmd

.LINK
Web Deploy Tool
http://technet.microsoft.com/en-us/library/dd568996(v=ws.10).aspx
#>
function Get-MSDeployCmd
{
    Write-VerboseWithTime 'Get-MSDeployCmd : début'
    $regKey = 'HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy'

    if (!(Test-Path $regKey))
    {
        throw ('Get-MSDeployCmd : introuvable ' + $regKey)
    }

    $versions = @(Get-ChildItem $regKey -ErrorAction SilentlyContinue)
    $lastestVersion =  $versions | Sort-Object -Property Name -Descending | Select-Object -First 1

    if ($lastestVersion)
    {
        $installPathKeys = 'InstallPath','InstallPath_x86'

        foreach ($installPathKey in $installPathKeys)
        {		    	
            $installPath = $lastestVersion.GetValue($installPathKey)

            if ($installPath)
            {
                $installPath = Join-Path $installPath -ChildPath 'MsDeploy.exe'

                if (Test-Path $installPath -PathType Leaf)
                {
                    $msdeployPath = $installPath
                    break
                }
            }
        }
    }

    Write-VerboseWithTime 'Get-MSDeployCmd : fin'
    return $msdeployPath
}


<#
.SYNOPSIS
Retourne $True lorsque l'URL est absolue et que son modèle est https.

.DESCRIPTION
La fonction Test-HttpsUrl convertit l'URL d'entrée en objet System.Uri. Retourne $True lorsque l'URL est absolue (non relative) et que son modèle est https. Si la valeur est false dans les deux cas, ou si la chaîne d'entrée ne peut pas être convertie en URL, la fonction retourne $false.

.PARAMETER Url
Spécifie l'URL à tester. Entrez une chaîne d'URL,

.INPUTS
AUCUNE.

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\>$profile.publishUrl
waws-prod-bay-001.publish.azurewebsites.windows.net:443

PS C:\>Test-HttpsUrl -Url 'waws-prod-bay-001.publish.azurewebsites.windows.net:443'
False

PS C:\>Test-HttpsUrl -Url 'https://waws-prod-bay-001.publish.azurewebsites.windows.net:443'
True
#>
function Test-HttpsUrl
{

    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $Url
    )

    # Si $uri ne peut pas être converti en objet System.Uri, Test-HttpsUrl retourne $false
    $uri = $Url -as [System.Uri]

    return $uri.IsAbsoluteUri -and $uri.Scheme -eq 'https'
}


<#
.SYNOPSIS
Déploie un package web vers Microsoft Azure.

.DESCRIPTION
La fonction Publish-WebPackage utilise MsDeploy.exe et un fichier ZIP de package de déploiement web pour déployer des ressources vers un site web Microsoft Azure. Cette fonction ne génère aucune sortie. En cas d'échec de l'appel à MSDeploy.exe, la fonction lève une exception. Pour obtenir une sortie plus détaillée, utilisez le paramètre commun Verbose.

.PARAMETER  WebDeployPackage
Spécifie le chemin d'accès et le nom de fichier d'un fichier ZIP de package de déploiement web généré par Visual Studio. Ce paramètre est obligatoire. Pour créer un fichier ZIP de package de déploiement web, voir les informations relatives à la création d'un package de déploiement web dans Visual Studio, à l'adresse suivante : http://go.microsoft.com/fwlink/?LinkId=391353.

.PARAMETER PublishUrl
Spécifie l'URL vers laquelle les ressources sont déployées. L'URL doit utiliser le protocole HTTPS et inclure le port. Ce paramètre est obligatoire.

.PARAMETER SiteName
Spécifie un nom pour le site web. Ce paramètre est obligatoire.

.PARAMETER Username
Spécifie le nom d'utilisateur de l'administrateur du site web. Ce paramètre est obligatoire.

.PARAMETER Password
Spécifie le mot de passe de l'administrateur du site web. Entrez un mot de passe en texte clair. Les chaînes sécurisées ne sont pas autorisées. Ce paramètre est obligatoire.

.PARAMETER AllowUntrusted
Autorise les connexions SSL non fiables au point de terminaison de Web Deploy. Ce paramètre est utilisé dans l'appel de MSDeploy.exe. Ce paramètre est facultatif.

.PARAMETER ConnectionString
Spécifie une chaîne de connexion pour une base de données SQL. Ce paramètre accepte une table de hachage avec les clés Name et ConnectionString. La valeur de Name correspond au nom de la base de données. La valeur de ConnectionString correspond à connectionStringName dans le fichier de configuration JSON.

.INPUTS
Aucune. Cette fonction n'accepte aucune entrée du pipeline.

.OUTPUTS
Aucun

.EXAMPLE
Publish-WebPackage -WebDeployPackage C:\Documents\Azure\ADWebApp.zip `
    -PublishUrl 'https://contoso.cloudapp.net:8172/msdeploy.axd' `
    -SiteName 'Site de test Contoso' `
    -UserName 'admin01' `
    -Password 'password' `
    -AllowUntrusted:$False `
    -ConnectionString @{Name="TestDB";ConnectionString="DefaultConnection"}

.LINK
Publish-WebPackageToVM

.LINK
Web Deploy Command Line Reference (MSDeploy.exe)
http://go.microsoft.com/fwlink/?LinkId=391354
#>
function Publish-WebPackage
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $WebDeployPackage,

        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-HttpsUrl $_ })]
        [String]
        $PublishUrl,

        [Parameter(Mandatory = $true)]
        [String]
        $SiteName,

        [Parameter(Mandatory = $true)]
        [String]
        $UserName,

        [Parameter(Mandatory = $true)]
        [String]
        $Password,

        [Parameter(Mandatory = $false)]
        [Switch]
        $AllowUntrusted = $false,

        [Parameter(Mandatory = $true)]
        [Hashtable]
        $ConnectionString
    )

    Write-VerboseWithTime 'Publish-WebPackage : début'

    $msdeployCmd = Get-MSDeployCmd

    if (!$msdeployCmd)
    {
        throw 'Publish-WebPackage : MsDeploy.exe est introuvable.'
    }

    $WebDeployPackage = (Get-Item $WebDeployPackage).FullName

    $msdeployCmd =  '"' + $msdeployCmd + '"'
    $msdeployCmd += ' -verb:sync'
    $msdeployCmd += ' -Source:Package="{0}"'
    $msdeployCmd += ' -dest:auto,computername="{1}?site={2}",userName={3},password={4},authType=Basic'
    if ($AllowUntrusted)
    {
        $msdeployCmd += ' -allowUntrusted'
    }
    $msdeployCmd += ' -setParam:name="IIS Web Application Name",value="{2}"'

    foreach ($DBConnection in $ConnectionString.GetEnumerator())
    {
        $msdeployCmd += (' -setParam:name="{0}",value="{1}"' -f $DBConnection.Key, $DBConnection.Value)
    }

    $msdeployCmd = $msdeployCmd -f $WebDeployPackage, $PublishUrl, $SiteName, $UserName, $Password
    $msdeployCmdForVerboseMessage = $msdeployCmd -f $WebDeployPackage, $PublishUrl, $SiteName, $UserName, '********'

    Write-VerboseWithTime ('Publish-WebPackage: MsDeploy: ' + $msdeployCmdForVerboseMessage)

    $msdeployExecution = Start-Process cmd.exe -ArgumentList ('/C "' + $msdeployCmd + '" ') -WindowStyle Normal -Wait -PassThru

    if ($msdeployExecution.ExitCode -ne 0)
    {
         Write-VerboseWithTime ('Msdeploy.exe s''est arrêté en raison d''une erreur. Code de sortie :' + $msdeployExecution.ExitCode)
    }

    Write-VerboseWithTime 'Publish-WebPackage : fin'
    return ($msdeployExecution.ExitCode -eq 0)
}


<#
.SYNOPSIS
Déploie une machine virtuelle vers Microsoft Azure.

.DESCRIPTION
La fonction Publish-WebPackageToVM est une fonction d'assistance qui vérifie les valeurs de paramètre avant d'appeler la fonction Publish-WebPackage.

.PARAMETER  VMDnsName
Spécifie le nom DNS de la machine virtuelle Microsoft Azure. Ce paramètre est obligatoire.

.PARAMETER IisWebApplicationName
Spécifie le nom d'une application web IIS pour la machine virtuelle. Ce paramètre est obligatoire. Nom de votre application web Visual Studio. Vous pouvez trouver le nom dans l'attribut webDeployparameters du fichier de configuration JSON généré par Visual Studio.

.PARAMETER WebDeployPackage
Spécifie le chemin d'accès et le nom de fichier d'un fichier ZIP de package de déploiement web généré par Visual Studio. Ce paramètre est obligatoire. Pour créer un fichier ZIP de package de déploiement web, consultez les informations relatives à la création d'un package de déploiement web dans Visual Studio, à l'adresse suivante : http://go.microsoft.com/fwlink/?LinkId=391353.

.PARAMETER Username
Spécifie le nom d'utilisateur de l'administrateur de la machine virtuelle. Ce paramètre est obligatoire.

.PARAMETER Password
Spécifie le mot de passe de l'administrateur de la machine virtuelle. Entrez un mot de passe en texte clair. Les chaînes sécurisées ne sont pas autorisées. Ce paramètre est obligatoire.

.PARAMETER AllowUntrusted
Autorise les connexions SSL non fiables au point de terminaison de Web Deploy. Ce paramètre est utilisé dans l'appel de MSDeploy.exe. Ce paramètre est facultatif.

.PARAMETER ConnectionString
Spécifie une chaîne de connexion pour une base de données SQL. Ce paramètre accepte une table de hachage avec les clés Name et ConnectionString. La valeur de Name correspond au nom de la base de données. La valeur de ConnectionString correspond à connectionStringName dans le fichier de configuration JSON.

.INPUTS
Aucune. Cette fonction n'accepte aucune entrée du pipeline.

.OUTPUTS
Aucun(e).

.EXAMPLE
Publish-WebPackageToVM -VMDnsName contoso.cloudapp.net `
-IisWebApplicationName myTestWebApp `
-WebDeployPackage C:\Documents\Azure\ADWebApp.zip
-Username 'admin01' `
-Password 'password' `
-AllowUntrusted:$False `
-ConnectionString @{Name="TestDB";ConnectionString="DefaultConnection"}

.LINK
Publish-WebPackage
#>
function Publish-WebPackageToVM
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $VMDnsName,

        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $IisWebApplicationName,

        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $WebDeployPackage,

        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $UserName,

        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [String]
        $UserPassword,

        [Parameter(Mandatory = $true)]
        [Bool]
        $AllowUntrusted,
        
        [Parameter(Mandatory = $true)]
        [Hashtable]
        $ConnectionString
    )
    Write-VerboseWithTime 'Publish-WebPackageToVM : début'

    $VMDnsUrl = $VMDnsName -as [System.Uri]

    if (!$VMDnsUrl)
    {
        throw ('Publish-WebPackageToVM : URL non valide ' + $VMDnsUrl)
    }

    $publishUrl = 'https://{0}:{1}/msdeploy.axd' -f $VMDnsUrl.Host, $WebDeployPort

    $result = Publish-WebPackage `
        -WebDeployPackage $WebDeployPackage `
        -PublishUrl $publishUrl `
        -SiteName $IisWebApplicationName `
        -UserName $UserName `
        -Password $UserPassword `
        -AllowUntrusted:$AllowUntrusted `
        -ConnectionString $ConnectionString

    Write-VerboseWithTime 'Publish-WebPackageToVM : fin'
    return $result
}


<#
.SYNOPSIS
Crée une chaîne qui vous permet de vous connecter à une base de données SQL Microsoft Azure.

.DESCRIPTION
La fonction Get-AzureSQLDatabaseConnectionString assemble une chaîne de connexion pour se connecter à une base de données SQL Microsoft Azure.

.PARAMETER  DatabaseServerName
Spécifie le nom d'un serveur de base de données existant dans l'abonnement Microsoft Azure. Toutes les bases de données SQL Microsoft Azure doivent être associées à un serveur de base de données SQL. Pour obtenir le nom du serveur, utilisez l'applet de commande Get-AzureSqlDatabaseServer (module Microsoft Azure). Ce paramètre est obligatoire.

.PARAMETER  DatabaseName
Spécifie le nom de la base de données SQL. Il peut s'agir d'une base de données SQL existante ou d'un nom utilisé pour une nouvelle base de données SQL. Ce paramètre est obligatoire.

.PARAMETER  Username
Spécifie le nom de l'administrateur de base de données SQL. Le nom d'utilisateur est $Username@DatabaseServerName. Ce paramètre est obligatoire.

.PARAMETER  Password
Spécifie le mot de passe de l'administrateur de base de données SQL. Entrez un mot de passe en texte clair. Les chaînes sécurisées ne sont pas autorisées. Ce paramètre est obligatoire.

.INPUTS
Aucun(e).

.OUTPUTS
System.String

.EXAMPLE
PS C:\> $ServerName = (Get-AzureSqlDatabaseServer).ServerName[0]
PS C:\> Get-AzureSQLDatabaseConnectionString -DatabaseServerName $ServerName `
        -DatabaseName 'testdb' -UserName 'admin'  -Password 'password'

Server=tcp:testserver.database.windows.net,1433;Database=testdb;User ID=admin@bebad12345;Password=password;Trusted_Connection=False;Encrypt=True;Connection Timeout=20;
#>
function Get-AzureSQLDatabaseConnectionString
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $DatabaseServerName,

        [Parameter(Mandatory = $true)]
        [String]
        $DatabaseName,

        [Parameter(Mandatory = $true)]
        [String]
        $UserName,

        [Parameter(Mandatory = $true)]
        [String]
        $Password
    )

    return ('Server=tcp:{0}.database.windows.net,1433;Database={1};' +
           'User ID={2}@{0};' +
           'Password={3};' +
           'Trusted_Connection=False;' +
           'Encrypt=True;' +
           'Connection Timeout=20;') `
           -f $DatabaseServerName, $DatabaseName, $UserName, $Password
}


<#
.SYNOPSIS
Crée des bases de données SQL Microsoft Azure à partir des valeurs du fichier de configuration JSON généré par Visual Studio.

.DESCRIPTION
La fonction Add-AzureSQLDatabases accepte les informations de la section databases du fichier JSON. Cette fonction, Add-AzureSQLDatabases (pluriel), appelle la fonction Add-AzureSQLDatabase (singulier) pour chaque base de données SQL du fichier JSON. Add-AzureSQLDatabase (singulier) appelle l'applet de commande New-AzureSqlDatabase (module Windows Azure), qui crée les bases de données SQL. Cette fonction ne retourne pas d'objet de base de données. Elle retourne une table de hachage des valeurs utilisées pour créer les bases de données.

.PARAMETER DatabaseConfig
 Accepte un tableau de PSCustomObjects qui proviennent du fichier JSON retourné par la fonction Read-ConfigFile quand le fichier JSON possède une propriété de site web. Cela inclut les propriétés de environmentSettings.databases. Vous pouvez utiliser le pipe de la liste pour cette fonction.
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases| where {$_.connectionStringName}
PS C:\> $DatabaseConfig
connectionStringName: Default Connection
databasename : TestDB1
edition   :
size     : 1
collation  : SQL_Latin1_General_CP1_CI_AS
servertype  : New SQL Database Server
servername  : r040tvt2gx
user     : dbuser
password   : Test.123
location   : West US

.PARAMETER  DatabaseServerPassword
Spécifie le mot de passe pour l'administrateur du serveur de base de données SQL. Entrez une table de hachage avec les clés Nom et Mot de passe. La valeur de Nom correspond au nom du serveur de base de données SQL. La valeur de Mot de passe correspond au mot de passe de l'administrateur. Par exemple : @Name = "TestDB1"; Password = "password" Ce paramètre est facultatif. Si vous l'omettez ou si le nom du serveur de base de données SQL ne correspond pas à la valeur de la propriété serverName de l'objet $DatabaseConfig la fonction utilise la propriété Mot de passe de l'objet $DatabaseConfig pour la base de données SQL dans la chaîne de connexion.

.PARAMETER CreateDatabase
Vérifie que vous souhaitez créer une base de données. Ce paramètre est facultatif.

.INPUTS
System.Collections.Hashtable[]

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases| where {$_.connectionStringName}
PS C:\> $DatabaseConfig | Add-AzureSQLDatabases

Name                           Value
----                           -----
ConnectionString               Server=tcp:testdb1.database.windows.net,1433;Database=testdb;User ID=admin@testdb1;Password=password;Trusted_Connection=False;Encrypt=True;Connection Timeout=20;
Name                           Default Connection
Type                           SQLAzure

.LINK
Get-AzureSQLDatabaseConnectionString

.LINK
Create-AzureSQLDatabase
#>
function Add-AzureSQLDatabases
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [PSCustomObject]
        $DatabaseConfig,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable[]]
        $DatabaseServerPassword,

        [Parameter(Mandatory = $false)]
        [Switch]
        $CreateDatabase = $false
    )

    begin
    {
        Write-VerboseWithTime 'Add-AzureSQLDatabases : début'
    }
    process
    {
        Write-VerboseWithTime ('Add-AzureSQLDatabases : création ' + $DatabaseConfig.databaseName)

        if ($CreateDatabase)
        {
            # Crée une base de données SQL avec les valeurs DatabaseConfig (à moins qu'elle n'existe déjà)
            # La sortie de la commande est supprimée.
            Add-AzureSQLDatabase -DatabaseConfig $DatabaseConfig | Out-Null
        }

        $serverPassword = $null
        if ($DatabaseServerPassword)
        {
            foreach ($credential in $DatabaseServerPassword)
            {
               if ($credential.Name -eq $DatabaseConfig.serverName)
               {
                   $serverPassword = $credential.password             
                   break
               }
            }               
        }

        if (!$serverPassword)
        {
            $serverPassword = $DatabaseConfig.password
        }

        return @{
            Name = $DatabaseConfig.connectionStringName;
            Type = 'SQLAzure';
            ConnectionString = Get-AzureSQLDatabaseConnectionString `
                -DatabaseServerName $DatabaseConfig.serverName `
                -DatabaseName $DatabaseConfig.databaseName `
                -UserName $DatabaseConfig.user `
                -Password $serverPassword }
    }
    end
    {
        Write-VerboseWithTime 'Add-AzureSQLDatabases : fin'
    }
}


<#
.SYNOPSIS
Crée une base de données SQL Microsoft Azure.

.DESCRIPTION
La fonction Add-AzureSQLDatabase crée une base de données SQL Microsoft Azure à partir des données du fichier de configuration JSON généré par Visual Studio, puis retourne la nouvelle base de données. Si l'abonnement a déjà une base de données SQL avec le nom de base de données spécifié sur le serveur de base de données SQL désigné, la fonction retourne la base de données existante. Cette fonction appelle l'applet de commande New-AzureSqlDatabase (module Microsoft Azure), qui crée en fait la base de données SQL.

.PARAMETER DatabaseConfig
Accepte un PSCustomObject qui provient du fichier de configuration JSON retourné par la fonction Read-ConfigFile lorsque le fichier JSON possède une propriété de site web. Cela inclut les propriétés de environmentSettings.databases. Vous ne pouvez pas utiliser le pipe de l'objet pour cette fonction. Visual Studio génère un fichier de configuration JSON pour tous les projets web et le stocke dans le dossier PublishScripts de votre solution.

.INPUTS
Aucune. Cette fonction n'accepte aucune entrée du pipeline

.OUTPUTS
Microsoft.WindowsAzure.Commands.SqlDatabase.Services.Server.Database

.EXAMPLE
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases | where connectionStringName
PS C:\> $DatabaseConfig

connectionStringName    : Default Connection
databasename : TestDB1
edition      :
size         : 1
collation    : SQL_Latin1_General_CP1_CI_AS
servertype   : New SQL Database Server
servername   : r040tvt2gx
user         : dbuser
password     : Test.123
location     : West US

PS C:\> Add-AzureSQLDatabase -DatabaseConfig $DatabaseConfig

.LINK
Add-AzureSQLDatabases

.LINK
New-AzureSQLDatabase
#>
function Add-AzureSQLDatabase
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [Object]
        $DatabaseConfig
    )

    Write-VerboseWithTime 'Add-AzureSQLDatabase : début'

    # Échec, si la valeur de paramètre n'a pas la propriété serverName, ou si la valeur de la propriété serverName n'est pas indiquée.
    if (-not (Test-Member $DatabaseConfig 'serverName') -or -not $DatabaseConfig.serverName)
    {
        throw 'Add-AzureSQLDatabase : le nom du serveur de base de données (obligatoire) est absent de la valeur DatabaseConfig.'
    }

    # Échec, si la valeur de paramètre n'a pas la propriété databasename, ou si la valeur de propriété databasename n'est pas indiquée.
    if (-not (Test-Member $DatabaseConfig 'databaseName') -or -not $DatabaseConfig.databaseName)
    {
        throw 'Add-AzureSQLDatabase : le nom de la base de données (obligatoire) est absent de la valeur DatabaseConfig.'
    }

    $DbServer = $null

    if (Test-HttpsUrl $DatabaseConfig.serverName)
    {
        $absoluteDbServer = $DatabaseConfig.serverName -as [System.Uri]
        $subscription = Get-AzureSubscription -Current -ErrorAction SilentlyContinue

        if ($subscription -and $subscription.ServiceEndpoint -and $subscription.SubscriptionId)
        {
            $absoluteDbServerRegex = 'https:\/\/{0}\/{1}\/services\/sqlservers\/servers\/(.+)\.database\.windows\.net\/databases' -f `
                                     $subscription.serviceEndpoint.Host, $subscription.SubscriptionId

            if ($absoluteDbServer -match $absoluteDbServerRegex -and $Matches.Count -eq 2)
            {
                 $DbServer = $Matches[1]
            }
        }
    }

    if (!$DbServer)
    {
        $DbServer = $DatabaseConfig.serverName
    }

    $db = Get-AzureSqlDatabase -ServerName $DbServer -DatabaseName $DatabaseConfig.databaseName -ErrorAction SilentlyContinue

    if ($db)
    {
        Write-HostWithTime ('Create-AzureSQLDatabase : utilisation de la base de données existante ' + $db.Name)
        $db | Out-String | Write-VerboseWithTime
    }
    else
    {
        $param = New-Object -TypeName Hashtable
        $param.Add('serverName', $DbServer)
        $param.Add('databaseName', $DatabaseConfig.databaseName)

        if ((Test-Member $DatabaseConfig 'size') -and $DatabaseConfig.size)
        {
            $param.Add('MaxSizeGB', $DatabaseConfig.size)
        }
        else
        {
            $param.Add('MaxSizeGB', 1)
        }

        # Si l'objet $DatabaseConfig a une propriété collation dont la valeur n'est pas Null ou vide
        if ((Test-Member $DatabaseConfig 'collation') -and $DatabaseConfig.collation)
        {
            $param.Add('Collation', $DatabaseConfig.collation)
        }

        # Si l'objet $DatabaseConfig a une propriété edition dont la valeur n'est pas Null ou vide
        if ((Test-Member $DatabaseConfig 'edition') -and $DatabaseConfig.edition)
        {
            $param.Add('Edition', $DatabaseConfig.edition)
        }

        # Écrire la table de hachage dans le flux des commentaires
        $param | Out-String | Write-VerboseWithTime
        # Appeler New-AzureSqlDatabase à l'aide de la projection (supprimer la sortie)
        $db = New-AzureSqlDatabase @param
    }

    Write-VerboseWithTime 'Add-AzureSQLDatabase : fin'
    return $db
}
