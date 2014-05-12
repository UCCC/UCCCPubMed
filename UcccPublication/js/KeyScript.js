        var sURL = unescape(window.location.pathname);

        function displayAlert(msg) {
            alert(msg);
        }

        function JS_GetImageHistology4Block(siteSeq, dateSeq, blockIdStr) {
            //alert("siteSeq=" + siteSeq);
            var imgBtnTemp = window.parent.document.getElementById("control" + dateSeq + "_" + siteSeq);
            if (imgBtnTemp != null) {
                //alert("control null");
                var temp = imgBtnTemp.src;
                //alert("imgBtnTemp.src=" + imgBtnTemp.src);       
                temp = temp.substring(0, temp.lastIndexOf("."));
                if (temp.charAt(temp.length - 1) == "p") {
                    temp = temp + ".gif";
                }
                else {
                    temp = temp + "p.gif";
                }
                imgBtnTemp.src = temp;
                //alert("imgBtnTemp.src2=" + imgBtnTemp.src);       
            }
            GetImagesWebService.GetImageHistology4Block(dateSeq, blockIdStr, SucceededCallback);
        }

        // This is the callback function that
        // processes the Web Service return value.
        function SucceededCallback(stringFromWebService) {
            //var collDate = document.getElementById("collDate1");
            var incomingStr = stringFromWebService;
            var tokenArray = incomingStr.split(";");
            var dateSeq = tokenArray[0];
            if (!tokenArray[1]) {
                alert("No BR or HI image available.");
                return;
            }
            var imageListArray = tokenArray[1];
            var siteCodeHistologyListArray = tokenArray[2];
            var labelListArray = tokenArray[3];
            var siteListArray = tokenArray[4];
            var imageArray = imageListArray.split(",");
            var siteCodeHistologyArray = siteCodeHistologyListArray.split(",");
            var labelArray = labelListArray.split(",");
            var siteArray = siteListArray.split(",");
            var i = 0;

            if (dateSeq == "1") {
                var leftImg = document.getElementById("leftImage");
                if (leftImg == null) {
                    //alert("leftImg=null2");
                }
                else {
                    //alert("leftImg found2");
                    leftImg.Visible = "false";
                    leftImg.src = "images/blank.jpg";
                }

                var childnode10 = document.getElementById("image10");
                if (childnode10) {
                    var removednode10 = document.getElementById("divimage1").removeChild(childnode10);
                }
                var childnodeHistology10 = document.getElementById("histology10");
                if (childnodeHistology10) {
                    var removednodehis10 = document.getElementById("divhistology1").removeChild(childnodeHistology10);
                }
                var childnode11 = document.getElementById("image11");
                if (childnode11) {
                    var removednode11 = document.getElementById("divimage1").removeChild(childnode11);
                }
                var childnodeHistology11 = document.getElementById("histology11");
                if (childnodeHistology11) {
                    var removednodehis11 = document.getElementById("divhistology1").removeChild(childnodeHistology11);
                }
                var childnode12 = document.getElementById("image12");
                if (childnode12) {
                    var removednode12 = document.getElementById("divimage1").removeChild(childnode12);
                }
                var childnodeHistology12 = document.getElementById("histology12");
                if (childnodeHistology12) {
                    var removednodehis12 = document.getElementById("divhistology1").removeChild(childnodeHistology12);
                }
                var childnode13 = document.getElementById("image13");
                if (childnode13) {
                    var removednode13 = document.getElementById("divimage1").removeChild(childnode13);
                }
                var childnodeHistology13 = document.getElementById("histology13");
                if (childnodeHistology13) {
                    var removednodehis13 = document.getElementById("divhistology1").removeChild(childnodeHistology13);
                }
                var childnode14 = document.getElementById("image14");
                if (childnode14) {
                    var removednode14 = document.getElementById("divimage1").removeChild(childnode14);
                }
                var childnodeHistology14 = document.getElementById("histology14");
                if (childnodeHistology14) {
                    var removednodehis14 = document.getElementById("divhistology1").removeChild(childnodeHistology14);
                }
                for (i in imageArray) {
                    if (i == 0) {
                        newImg10 = document.createElement('div');
                        var imageId10 = "image10";
                        newImg10.id = imageId10;
                        var imageName10 = imageArray[i];
                        var site10 = siteArray[i];
                        //alert(site10);
                        newImg10.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName10 + ".jpg>";
                        document.getElementById("divimage1").appendChild(newImg10);
                        var imagePath10 = "organmap_image/" + imageName10 + ".jpg";
                        var obj10 = document.getElementById(imageId10).onclick = function () {
                            var leftImg10 = document.getElementById("leftImage");
                            leftImg10.Visible = "true";
                            leftImg10.src = imagePath10;
                        };

                        newHistology10 = document.createElement('div');
                        var histologyId10 = "histology10";
                        newHistology10.id = histologyId10;
                        var histologyName10 = siteCodeHistologyArray[i]
                        newHistology10.innerHTML = "<font face='Arial' size=1>" + histologyName10 + "</font>";
                        document.getElementById("divhistology1").appendChild(newHistology10);
                    }
                    else if (i == 1) {
                        newImg11 = document.createElement('div');
                        var imageId11 = "image11";
                        newImg11.id = imageId11;
                        var imageName11 = imageArray[i]
                        var site11 = siteArray[i];
                        //alert(site11);
                        newImg11.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName11 + ".jpg>";
                        document.getElementById("divimage1").appendChild(newImg11);
                        var imagePath11 = "organmap_image/" + imageName11 + ".jpg";
                        var obj11 = document.getElementById(imageId11).onclick = function () {
                            var leftImg11 = document.getElementById("leftImage");
                            leftImg11.Visible = "true";
                            leftImg11.src = "organmap_image/" + imageName11 + ".jpg";
                        };

                        newHistology11 = document.createElement('div');
                        var histologyId11 = "histology11";
                        newHistology11.id = histologyId11;
                        var histologyName11 = siteCodeHistologyArray[i]
                        newHistology11.innerHTML = "<font face='Arial' size=1>" + histologyName11 + "</font>";
                        document.getElementById("divhistology1").appendChild(newHistology11);

                    }
                    else if (i == 2) {
                        newImg12 = document.createElement('div');
                        var imageId12 = "image12";
                        newImg12.id = imageId12;
                        var imageName12 = imageArray[i]
                        var site12 = siteArray[i];
                        //alert(site12);
                        newImg12.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName12 + ".jpg>";
                        document.getElementById("divimage1").appendChild(newImg12);
                        var imagePath12 = "organmap_image/" + imageName12 + ".jpg";
                        var obj12 = document.getElementById(imageId12).onclick = function () {
                            var leftImg12 = document.getElementById("leftImage");
                            leftImg12.Visible = "true";
                            leftImg12.src = "organmap_image/" + imageName12 + ".jpg";
                        };

                        newHistology12 = document.createElement('div');
                        var histologyId12 = "histology12";
                        newHistology12.id = histologyId12;
                        var histologyName12 = siteCodeHistologyArray[i]
                        newHistology12.innerHTML = "<font face='Arial' size=1>" + histologyName12 + "</font>";
                        document.getElementById("divhistology1").appendChild(newHistology12);
                    }
                    else if (i == 3) {
                        newImg13 = document.createElement('div');
                        var imageId13 = "image13";
                        newImg13.id = imageId13;
                        var imageName13 = imageArray[i]
                        newImg13.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName13 + ".jpg>";
                        document.getElementById("divimage1").appendChild(newImg13);
                        var imagePath13 = "organmap_image/" + imageName13 + ".jpg";
                        var obj13 = document.getElementById(imageId13).onclick = function () {
                            var leftImg13 = document.getElementById("leftImage");
                            leftImg13.Visible = "true";
                            leftImg13.src = "organmap_image/" + imageName13 + ".jpg";
                        };

                        newHistology13 = document.createElement('div');
                        var histologyId13 = "histology13";
                        newHistology13.id = histologyId13;
                        var histologyName13 = siteCodeHistologyArray[i]
                        newHistology13.innerHTML = "<font face='Arial' size=1>" + histologyName13 + "</font>";
                        document.getElementById("divhistology1").appendChild(newHistology13);
                    }
                    else if (i == 4) {
                        newImg14 = document.createElement('div');
                        var imageId14 = "image14";
                        newImg14.id = imageId14;
                        var imageName14 = imageArray[i]
                        newImg14.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName14 + ".jpg>";
                        document.getElementById("divimage1").appendChild(newImg14);
                        var imagePath14 = "organmap_image/" + imageName14 + ".jpg";
                        var obj14 = document.getElementById(imageId14).onclick = function () {
                            var leftImg14 = document.getElementById("leftImage");
                            leftImg14.Visible = "true";
                            leftImg14.src = "organmap_image/" + imageName14 + ".jpg";
                        };

                        newHistology14 = document.createElement('div');
                        var histologyId14 = "histology14";
                        newHistology14.id = histologyId14;
                        var histologyName14 = siteCodeHistologyArray[i]
                        newHistology14.innerHTML = "<font face='Arial' size=1>" + histologyName14 + "</font>";
                        document.getElementById("divhistology1").appendChild(newHistology14);
                    }
                }
            }
            else {
                var rightImg = document.getElementById("rightImage");
                if (rightImg == null) {
                    //alert("rightImg2=null");
                }
                else {
                    //alert("rightImg found2");
                    rightImg.Visible = "false";
                    rightImg.src="images/blank.jpg"
                }
           
                var childnode20 = document.getElementById("image20");
                if (childnode20) {
                    var removednode = document.getElementById("divimage2").removeChild(childnode20);
                }
                var childnodeHistology20 = document.getElementById("histology20");
                if (childnodeHistology20) {
                    var removednode = document.getElementById("divhistology2").removeChild(childnodeHistology20);
                }
                var childnode21 = document.getElementById("image21");
                if (childnode21) {
                    var removednode = document.getElementById("divimage2").removeChild(childnode21);
                }
                var childnodeHistology21 = document.getElementById("histology21");
                if (childnodeHistology21) {
                    var removednode = document.getElementById("divhistology2").removeChild(childnodeHistology21);
                }
                var childnode22 = document.getElementById("image22");
                if (childnode22) {
                    var removednode = document.getElementById("divimage2").removeChild(childnode22);
                }
                var childnodeHistology22 = document.getElementById("histology22");
                if (childnodeHistology22) {
                    var removednode = document.getElementById("divhistology2").removeChild(childnodeHistology22);
                }
                var childnode23 = document.getElementById("image23");
                if (childnode23) {
                    var removednode = document.getElementById("divimage2").removeChild(childnode23);
                }
                var childnodeHistology23 = document.getElementById("histology23");
                if (childnodeHistology23) {
                    var removednode = document.getElementById("divhistology2").removeChild(childnodeHistology23);
                }
                var childnode24 = document.getElementById("image24");
                if (childnode24) {
                    var removednode = document.getElementById("divimage2").removeChild(childnode24);
                }
                var childnodeHistology24 = document.getElementById("histology24");
                if (childnodeHistology24) {
                    var removednode = document.getElementById("divhistology2").removeChild(childnodeHistology24);
                }
                for (i in imageArray) {
                    if (i == 0) {
                        newImg20 = document.createElement('div');
                        var imageId20 = "image20";
                        newImg20.id = imageId20;
                        var imageName20 = imageArray[i]
                        newImg20.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName20 + ".jpg>";
                        document.getElementById("divimage2").appendChild(newImg20);
                        var imagePath20 = "organmap_image/" + imageName20 + ".jpg";
                        var obj20 = document.getElementById(imageId20).onclick = function () {
                            var leftImg20 = document.getElementById("rightImage");
                            leftImg20.Visible = "true";
                            leftImg20.src = "organmap_image/" + imageName20 + ".jpg";
                        };

                        newHistology20 = document.createElement('div');
                        var histologyId20 = "histology20";
                        newHistology20.id = histologyId20;
                        var histologyName20 = siteCodeHistologyArray[i]
                        newHistology20.innerHTML = "<font face='Arial' size=1>" + histologyName20 + "</font>";
                        document.getElementById("divhistology2").appendChild(newHistology20);
                    }
                    else if (i == 1) {
                        newImg21 = document.createElement('div');
                        var imageId21 = "image21";
                        newImg21.id = imageId21;
                        var imageName21 = imageArray[i]
                        newImg21.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName21 + ".jpg>";
                        document.getElementById("divimage2").appendChild(newImg21);
                        var imagePath21 = "organmap_image/" + imageName21 + ".jpg";
                        var obj21 = document.getElementById(imageId21).onclick = function () {
                            var leftImg21 = document.getElementById("rightImage");
                            leftImg21.Visible = "true";
                            leftImg21.src = "organmap_image/" + imageName21 + ".jpg";
                        };

                        newHistology21 = document.createElement('div');
                        var histologyId21 = "histology21";
                        newHistology21.id = histologyId21;
                        var histologyName21 = siteCodeHistologyArray[i]
                        newHistology21.innerHTML = "<font face='Arial' size=1>" + histologyName21 + "</font>";
                        document.getElementById("divhistology2").appendChild(newHistology21);
                    }
                    else if (i == 2) {
                        newImg22 = document.createElement('div');
                        var imageId22 = "image22";
                        newImg22.id = imageId22;
                        var imageName22 = imageArray[i]
                        newImg22.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName22 + ".jpg>";
                        document.getElementById("divimage2").appendChild(newImg22);
                        var imagePath22 = "organmap_image/" + imageName22 + ".jpg";
                        var obj22 = document.getElementById(imageId22).onclick = function () {
                            var leftImg22 = document.getElementById("rightImage");
                            leftImg22.Visible = "true";
                            leftImg22.src = "organmap_image/" + imageName22 + ".jpg";
                        };

                        newHistology22 = document.createElement('div');
                        var histologyId22 = "histology22";
                        newHistology22.id = histologyId22;
                        var histologyName22 = siteCodeHistologyArray[i]
                        newHistology22.innerHTML = "<font face='Arial' size=1>" + histologyName22 + "</font>";
                        document.getElementById("divhistology2").appendChild(newHistology22);
                    }
                    else if (i == 3) {
                        newImg23 = document.createElement('div');
                        var imageId23 = "image23";
                        newImg23.id = imageId23;
                        var imageName23 = imageArray[i]
                        newImg23.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName23 + ".jpg>";
                        document.getElementById("divimage2").appendChild(newImg23);
                        var imagePath23 = "organmap_image/" + imageName23 + ".jpg";
                        var obj23 = document.getElementById(imageId23).onclick = function () {
                            var leftImg23 = document.getElementById("rightImage");
                            leftImg23.Visible = "true";
                            leftImg23.src = "organmap_image/" + imageName23 + ".jpg";
                        };

                        newHistology23 = document.createElement('div');
                        var histologyId23 = "histology23";
                        newHistology23.id = histologyId23;
                        var histologyName23 = siteCodeHistologyArray[i]
                        newHistology23.innerHTML = "<font face='Arial' size=1>" + histologyName23 + "</font>";
                        document.getElementById("divhistology2").appendChild(newHistology23);
                    }
                    else if (i == 4) {
                        newImg24 = document.createElement('div');
                        var imageId24 = "image24";
                        newImg24.id = imageId24;
                        var imageName24 = imageArray[i]
                        newImg24.innerHTML = "<img width=40 height=30 src=organmap_image/thumbnail/" + imageName24 + ".jpg>";
                        document.getElementById("divimage2").appendChild(newImg24);
                        var imagePath24 = "organmap_image/" + imageName24 + ".jpg";
                        var obj24 = document.getElementById(imageId24).onclick = function () {
                            var leftImg24 = document.getElementById("rightImage");
                            leftImg24.Visible = "true";
                            leftImg24.src = "organmap_image/" + imageName24 + ".jpg";
                        };

                        newHistology24 = document.createElement('div');
                        var histologyId24 = "histology24";
                        newHistology24.id = histologyId24;
                        var histologyName24 = siteCodeHistologyArray[i]
                        newHistology24.innerHTML = "<font face='Arial' size=1>" + histologyName24 + "</font>";
                        document.getElementById("divhistology2").appendChild(newHistology24);
                    }
                }
            }
            /* example:
            newimg=document.createElement('img');
            newimg.style.display='block';
            newimg.onclick=function(){this.parentNode.removeChild(this);};
            newimg.src=o.href;
            o.parentNode.appendChild(newimg)
            */

            //alert(imageArray[0]+" "+imageArray[1]+" "+imageArray[2]);
            /*
            //var type = Sys.Preview.UI.Image;
            var properties = { imageURL: "images/stripesblue.jpg", alternateText: "test text", width:155, height:58};
            var events = null;
            var references = null;
            var element = $get("thumbnail1_1");
            $create(type, properties, events, references, element);
            */
            var ctrl1 = document.getElementById("control1_1");
            if (ctrl1 != null) {
                //ctrl1.parentNode.removeChild(ctrl1); 
                //alert ("ctrl1 is here");
                ctrl1.ImageUrl = "images/ball16.gif";
            }

        }
        function getParameter(queryString, parameterName) {
            // Add "=" to the parameter name (i.e. parameterName=value)
            var parameterName = parameterName + "=";
            if (queryString.length > 0) {
                // Find the beginning of the string
                begin = queryString.indexOf(parameterName);
                // If the parameter name is not found, skip it, otherwise return the value
                if (begin != -1) {
                    // Add the length (integer) to the beginning
                    begin += parameterName.length;
                    // Multiple parameters are separated by the "&" sign
                    end = queryString.indexOf("&", begin);
                    if (end == -1) {
                        end = queryString.length
                    }
                    // Return the string
                    return unescape(queryString.substring(begin, end));
                }
                // Return "null" if no parameter has been found
                return "null";
            }
        }
        function displayDates() {
            //var collDate = document.getElementById("collDate1");
            var queryString = window.top.location.search.substring(1);
            //collDate.innerHTML = document.Request.QueryString("date1");
            //collDate.innerHTML = "Collection date: " + getParameter(queryString,"date1");

            // var collDate2 = document.getElementById("collDate2");
            var queryString2 = window.top.location.search.substring(1);
            //collDate.innerHTML = document.Request.QueryString("date1");
            //collDate2.innerHTML = "Collection date: " + getParameter(queryString,"date2");

        }
        function klik() {
            alert("ppp");
        }

        function addNewImg(newimageId) {
            var newImg = document.createElement('div');
            /*
            newImg = document.createelement('img'); 
            newImg.id = "image"+newimageId; 
            newImg = document.getElementById("divimage1").appendChild(newImg); 
            newImg.onclick=klik 
            */
        } 


