<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebApplicationMongoDB.View.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>View your mongoDB databse online</title>
    <link href="Image/icon/database-16.png" rel="shortcut icon" />
    <link rel="stylesheet" href="Style/bootstrap/bootstrap.css" />
    <link rel="stylesheet" href="Style/bootstrap/bootstrap-theme.css" />
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,300' rel='stylesheet' type='text/css' />
    <link rel="stylesheet" href="Style/Main.css" />
</head>
<body id="body" runat="server">
    <header id="header">
        <div class="conatain-logo">
            <img class="logo" src="Image/logo.png" alt="logo" style="width: 4%;" />
        </div>
        <div id="myCarousel" class="carousel slide" data-ride="carousel" style="margin-top: 4%;">
            <!-- Indicators -->
            <ol class="carousel-indicators">
                <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                <li data-target="#myCarousel" data-slide-to="1"></li>
                <li data-target="#myCarousel" data-slide-to="2"></li>
            </ol>
            <div class="carousel-inner" role="listbox" id="carrousel">
                <div class="item active">

                    <img class="first-slide img" src="Image/bg.jpg" alt="First slide" style="width: 100%; height: 100%" />
                    <div class="contain">
                        <h1><span>Lorem</span> ipsum</h1>
                        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. </p>
                        <p>Et quoniam mirari posse quosdam peregrinos </p>
                        <div>
                            <a class="btn-carrousel" style="transition-delay: 0.3s">Inscription</a>
                        </div>
                        <div>
                            <a class="btn-carrousel" style="transition-delay: 0.3s">Connection</a>
                        </div>
                    </div>
                </div>
                <div class="item">
                    <img class="second-slide img" src="Image/Photo24.jpg" alt="Second slide" style="width: 100%; height: 100%" />
                    <div class="contain">
                        <h1><span>Lorem</span> ipsum</h1>
                        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. </p>
                        <p>Et quoniam mirari posse quosdam peregrinos </p>
                        <div>
                            <a class="btn-carrousel" style="transition-delay: 0.3s">Inscription</a>
                        </div>
                        <div>
                            <a class="btn-carrousel" style="transition-delay: 0.3s">Connection</a>
                        </div>
                    </div>
                </div>
                <div class="item">
                    <img class="third-slide img" src="Image/bg.jpg" alt="Third slide" style="width: 100%; height: 100%" />
                    <div class="contain">
                        <h1><span>Lorem</span> ipsum</h1>
                        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. </p>
                        <p>Et quoniam mirari posse quosdam peregrinos </p>
                        <div>
                            <a class="btn-carrousel" style="transition-delay: 0.3s">Inscription</a>
                        </div>
                        <div>
                            <a class="btn-carrousel" style="transition-delay: 0.3s">Connection</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <section class="sct-contain">
        <article class="row" style="text-align: center;" id="article1">
            <div class="col-lg-4">
                <img src="Image/icon/phone-72-64.png" />
                <h3>Lorem ipsum</h3>
                <p>Sed tamen haec cum ita tutius observentur, quidam vigore artuum inminuto rogati ad nuptias...</p>
                <a class="yellow" href="#article2" style="text-decoration: none!important">En savoir plus &raquo;</a>
            </div>
            <div class="col-lg-4">
                <img src="Image/icon/calendar-7-64.png" />
                <h3>Lorem ipsum</h3>
                <p>Sed tamen haec cum ita tutius observentur, quidam vigore artuum inminuto rogati ad nuptias...</p>
                <a class="green" href="#article3" style="text-decoration: none!important">En savoir plus &raquo;</a>
            </div>
            <div class="col-lg-4">
                <img src="Image/icon/clock-3-64 (1).png" />
                <h3>Lorem ipsum</h3>
                <p>Sed tamen haec cum ita tutius observentur, quidam vigore artuum inminuto rogati ad nuptias...</p>
                <a class="orange" href="#social" style="text-decoration: none!important">En savoir plus &raquo;</a>
            </div>
        </article>
        <hr style="margin-bottom: 15px;" />
        <form class="navbar-form navbar-left" role="search" style="margin: 2% 41%">
            <div class="form-group">
                <input type="text" class="form-control" placeholder="Search" />
            </div>
            <%--<button type="submit" class="btn btn-default">Submit</button>--%>
        </form>
    </section>

    <section class="containerStock" id="containerStock" runat="server">
    </section>
    <section class="sct-contain">
        <hr style="margin-bottom: 15px; margin-top:4%" />
        <div class="social">
            <img src="Image/social/facebook-4-48.png" />
            <img src="Image/social/twitter-4-48.png" />
            <img src="Image/social/linkedin-4-48.png" />
        </div>
        <hr style="margin-top: 10px;" />
        <article id="article2">
            <div class="article right">
                <h1 class="title">Lorem <span>ipsm</span></h1>
                <h4>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.
                </h4>
                <p>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, 
                        quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                </p>

            </div>
            <div class="pic pic-left">
                <img src="Image/icon/phone-72-256 (1).png" />
            </div>
        </article>
        <hr style="clear: both;" />
        <article id="article3">
            <div class="article left">
                <h1 class="title">Lorem <span>ipsm</span></h1>
                <h4>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.
                </h4>
                <p>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, 
                        quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                </p>
            </div>
            <div class="pic pic-right">
                <img src="Image/icon/calendar-7-256 (1).png" />
            </div>
        </article>
        <hr style="clear: both;" />
        <article>
            <div class="article right" id="social">
                <h1 class="title">Lorem <span>ipsm</span></h1>
                <h4>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.
                </h4>
                <p>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, 
                        quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                </p>

            </div>
            <div class="pic pic-left">
                <img src="Image/icon/clock-3-256.png" />
            </div>
        </article>

    </section>


    <footer>
        <article class="row" style="text-align: center;">
            <div class="col-lg-4 footer">
                <h4>Navigation</h4>
                <a href="#header" style="text-decoration: none!important">Acceuil</a>
                <a>Contact</a>
                <a>Inscription</a>
                <a>Connection</a>
            </div>
            <div class="col-lg-4 footer">
                <h4>Lorem</h4>
                <a href="#article2" style="text-decoration: none!important">Lorem ipsum</a>
                <a href="#article3" style="text-decoration: none!important">Lorem ispum</a>
                <a href="#social" style="text-decoration: none!important">Lorem ispul</a>
            </div>
            <div class="col-lg-4 footer">
                <h4>Social</h4>
                <a>Facebook</a>
                <a>twitter</a>
                <a>Linkedin</a>
            </div>
        </article>
        <hr style="margin-bottom: 7px!important;" />
        <div class="social footer" style="margin-top: auto!important">
            <img src="Image/social/facebook-4-32.png" />
            <img src="Image/social/twitter-4-32.png" />
            <img src="Image/social/linkedin-4-32.png" />
        </div>
        <hr style="margin-top: 2px;" />
    </footer>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <%--<script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.14.0/jquery.validate.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.13.0/additional-methods.min.js"></script>--%>
    <script src="Script/bootstrap/bootstrap.js"></script>
    <script src="Script/bootstrap/bootstrap.js"></script>
    <script src="Script/main.js"></script>
    <script src="Script/validation.js"></script>
</body>
</html>
