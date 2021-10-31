var spawnBirds = true;

class DOMOperations {
    static initCarousel() {
        $(document).ready(function () {
            $(".owl-carousel").owlCarousel({
                margin: 5,
                // center:true,
                loop: true,
                responsiveClass: true,
                responsive: {
                    0: {
                        items: 1,
                        nav: true
                    },
                    600: {
                        items: 2,
                        nav: false
                    },
                    1200: {
                        items: 3,
                        nav: true,
                        loop: false
                    }
                }
            });
        });
    }

    static showNotificationRequest() {
        var backdrop = $(".backdrop");
        var modal = $(".karga-notifications-popup");
        backdrop.show();
        modal.show();
    }

    static initializeTooltips() {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl)
        })
    }

    static openChatWindow(){
        if (window.innerWidth > 768) {
            $(".karga-chat-overlay").css("display", "block");
            $(".chat-box").css("display", "block")
        }
        else{
            window.location.href = '/mobile/chat?returnUrl='+window.location.pathname;
        }
    }

    static initEmbeds(){
        var embeds = $(".wp-block-embed__wrapper");
        for(var i = 0; i < embeds.length; i++){
            var embed = embeds[i];

            var wrapper = document.createElement("div")
            wrapper.className = 'jetpack-video-wrapper';

            var iframe = document.createElement("iframe");

            var url = 'https://www.youtube.com/embed/'+getId(embed.innerText);
            iframe.src = url
            iframe.allowFullscreen = true;
            iframe.width = '100%'
            iframe.height = '400px'
            wrapper.append(iframe)

            embed.innerHTML = '';
            embed.append(wrapper)
        }
    }

    static loadDisqusCommentStatus(postId){
        $.ajax({
            url:'https://disqus.com/api/3.0/threads/details.json',
            method:'GET',
            data:{
                api_key:'lRYpJkl2s0rLS4xNMqhICM7ilsYpha6c4tv1OtzmLcJomaq6br5Wa3g1dck807x7',
                forum:'kargala',
                thread:'ident:'+postId
            },
            success:function(e){
                $("#comments_count_"+postId)[0].innerText = e.response.posts;
                console.log(e.response.posts);
            },
            error:function(e){
                $("#comments_count_"+postId)[0].innerText = "0";
                console.log(e);
            }
        })
    }

    static setBirds(){
        setInterval(()=>{

            if(spawnBirds && !disabledBirdsForPage)
            addBirds();
        },15e3)
    }

    static toggleSearch(){
        var searchModal = new bootstrap.Modal(document.getElementById('searchModal'))
        searchModal.toggle();
    }
}

function getId(url) {
    const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/;
    const match = url.match(regExp);

    return (match && match[2].length === 11)
      ? match[2]
      : null;
}  


class Karga {
    constructor() {
        console.log("%cKargala'ya Hoş Geldin Sayın Meraklı Kullanıcı!", "color:lime;font-size:16px;", "\n\nSanırım seni buradan yapabileceklerin hakkında uyarmamız gerekiyor.\nBuradan yapabileceğin bütün işlemler sistemimiz tarafından kayıt edilmektedir.\nLütfen anlamsız hackerlık işlerine bulaşma.\n\nAyrıca haberlerimizin paylaşımını arttırmak amacıyla sitemizde resmi bir REST API mevcut. Tam da burada!");
    }
}

var karga = new Karga();
// DOMOperations.showNotificationRequest();
DOMOperations.initCarousel();
DOMOperations.initializeTooltips();
DOMOperations.initEmbeds();
DOMOperations.setBirds();

window.addEventListener('focus',()=>{
    spawnBirds = true;
    console.warn("Spawn birds enabled!");
})

window.addEventListener('blur',()=>{
    spawnBirds = false;
    console.warn("Spawn birds disabled!");
})

if(spawnBirds && !disabledBirdsForPage)
addBirds();