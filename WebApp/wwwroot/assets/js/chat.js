/*
    Chat-Extension For Karga.La
*/
var myModal = new bootstrap.Modal(document.getElementById('selectUsernameModal'))

var connection = new signalR.HubConnectionBuilder().withUrl("/live/chat").build();
var username = getCookie('INet_UserName')
var wait = false;
var INDEX = 0; 

var notificationSound = new Audio('/assets/sound/ding.wav');

connection.start();

connection.on('ReceiveMessage',(user,name)=>{
    var type = "user";
    if(user == username){
        type = "self";
        user = 'Siz';
    }
    generate_message(name,type,user)
    notificationSound.play();
})

function saveUsername(usernamea){
  console.log(usernamea.trim().length);
    if(usernamea.trim().includes(' ') || usernamea.trim().length > 10){
        console.log("No!");
        alert("Lütfen kullanıcı adınızda boşluk kullanmayınız ve kullanıcı adınızın en fazla 10 haneli olduğundan emin olunuz.")
        return;
    }
    username = usernamea.trim();
    myModal.hide();
    setCookie('INet_UserName',usernamea.trim(),365);
}

function dateFormat(input){
  if(input < 10){
    return "0"+input;
  }
  return ""+input;
}

function generate_message(msg, type, username) {
    INDEX++;
    var str="";
    str += "<div id='cm-msg-"+INDEX+"' class=\"chat-msg "+type+"\">";
    str += "         ";
    str += "          <div class=\"cm-msg-text\"><span class='user-name'>"+username+"</span><span class=\"my-content\"></span>";
    str += "         <span class='time'>"+dateFormat(new Date().getHours())+":"+dateFormat(new Date().getMinutes())+"</span> <\/div>";
    str += "        <\/div>";
    $(".chat-logs").append(str);
    $("#cm-msg-"+INDEX).hide().fadeIn(300);
    if(type == 'self'){
     $("#chat-input").val(''); 
    }    
    $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight}, 1000);    
    $("#cm-msg-"+INDEX)[0].getElementsByClassName("my-content")[0].append(document.createTextNode(msg))
  }  

$(function() {
    $("#chat-submit").click(function(e) {
      e.preventDefault();

      if(wait){
        alert("Çok hızlı mesaj gönderiyorsun. Biraz yavaşla.")
        return;
      }

      if(username == null){
        console.log("Username is null!");
        myModal.toggle();
        return;
      }

      var msg = $("#chat-input").val(); 
      if(msg.trim() == ''){
        return false;
      }

      console.log(msg.trim().length);

      if(msg.trim().length > 300){
        alert("Mesajınız çok uzun. Lütfen anlatmak istediğinizi biraz kısaltarak yazınız.");
        return false;
      }

      generate_message(msg.trim(), 'self','Siz');
      connection.invoke("SendMessage", username , msg.trim()).catch(function (err) {
        return console.error(err.toString());
      });
      wait = true;
      setTimeout(()=>{
        wait = false;
      },1000)
    })
    

    
    function generate_button_message(msg, buttons){    
      /* Buttons should be object array 
        [
          {
            name: 'Existing User',
            value: 'existing'
          },
          {
            name: 'New User',
            value: 'new'
          }
        ]
      */
      INDEX++;
      var btn_obj = buttons.map(function(button) {
         return  "              <li class=\"button\"><a href=\"javascript:;\" class=\"btn btn-primary chat-btn\" chat-value=\""+button.value+"\">"+button.name+"<\/a><\/li>";
      }).join('');
      var str="";
      str += "<div id='cm-msg-"+INDEX+"' class=\"chat-msg user\">";
      str += "          <span class=\"msg-avatar\">";
      str += "            <img src=\"https:\/\/image.crisp.im\/avatar\/operator\/196af8cc-f6ad-4ef7-afd1-c45d5231387c\/240\/?1483361727745\">";
      str += "          <\/span>";
      str += "          <div class=\"cm-msg-text\">";
      str += msg;
      str += "          <\/div>";
      str += "          <div class=\"cm-msg-button\">";
      str += "            <ul>";   
      str += btn_obj;
      str += "            <\/ul>";
      str += "          <\/div>";
      str += "        <\/div>";
      $(".chat-logs").append(str);
      $("#cm-msg-"+INDEX).hide().fadeIn(300);   
      $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight}, 1000);
      $("#chat-input").attr("disabled", true);
    }
    
    $(document).delegate(".chat-btn", "click", function() {
      var value = $(this).attr("chat-value");
      var name = $(this).html();
      $("#chat-input").attr("disabled", false);
      generate_message(name, 'self');
    })

    $(".chat-box-toggle").click(function() {
        $("#chat-circle").toggle('scale');
        $(".chat-box").toggle('scale');
        if(returnTo != null){
          window.location.href = returnTo;
        }
      })
    
  })

if(window.innerWidth > 768)
  $(function(){
    var mousePosition;
    var offset = [0,0];
    var div;
    var isDown = false;
    
    div = $(".chat-box")[0];
    // div.style.position = "absolute";
    // div.style.left = "0px";
    // div.style.top = "0px";
    
    div.addEventListener('mousedown', function(e) {
        isDown = true;
        offset = [
            div.offsetLeft - e.clientX,
            div.offsetTop - e.clientY
        ];
    }, false);
    
    document.addEventListener('mouseup', function() {
        isDown = false;
    }, true);
    
    document.addEventListener('mousemove', function(event) {
        event.preventDefault();
        if (isDown) {
            mousePosition = {
    
                x : event.clientX,
                y : event.clientY
    
            };
            div.style.left = (mousePosition.x + offset[0]) + 'px';
            div.style.top  = (mousePosition.y + offset[1]) + 'px';
        }
    }, true);
})

function setCookie(cname, cvalue, exdays) {
  const d = new Date();
  d.setTime(d.getTime() + (exdays*24*60*60*1000));
  let expires = "expires="+ d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
  let name = cname + "=";
  let decodedCookie = decodeURIComponent(document.cookie);
  let ca = decodedCookie.split(';');
  for(let i = 0; i <ca.length; i++) {
    let c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return null;
}

window.addEventListener('load',()=>{
  $("#logs-panel")[0].scrollTo(0,10e10)
})