var cookieValue = getCookie('react_' + currentPost);
var postReact = cookieValue == null ? 'nothing' : cookieValue;
var popoverPositions = [];
function like() {
    if (postReact == 'nothing') {
        $.ajax({
            url: '/api/Core/Posts/Like?id=' + currentPost,
            method: 'GET',
            success: function () {
                setCookie('react_' + currentPost, 'liked', 365)
                postReact = 'liked';
                setLikeStatus();
            }
        })
    }
}

function dislike() {
    if (postReact == 'nothing') {
        $.ajax({
            url: '/api/Core/Posts/Dislike?id=' + currentPost,
            method: 'GET',
            success: function () {
                setCookie('react_' + currentPost, 'disliked', 365)
                postReact = 'disliked'
                setLikeStatus();
            }
        })
    }
}

function setLikeStatus() {
    if (postReact == 'liked') {
        $(".karga-section-liked").css('display', 'block')
        $(".karga-section-like").css('display', 'none')
    }
    else if (postReact == 'disliked') {
        $(".karga-section-disliked").css('display', 'block')
        $(".karga-section-like").css('display', 'none')
    }
}

$(function () {
    setLikeStatus();
})

$(function () {

    function setPopoverLocations() {
        var pageHeight = document.body.scrollHeight - window.innerHeight - 300;
        var location1 = pageHeight / 3;
        var location2 = pageHeight * 2 / 3;
        var location3 = pageHeight;
        popoverPositions.push(location1, location2, location3);
    }

    setPopoverLocations()

    var oneTriggered = false;
    var twoTriggered = false;
    var threeTriggered = false;

    $('body').scroll(function (e) {
        const currentPosition = document.body.scrollTop;

        if (currentPostition > popoverPositions[0] && !oneTriggered) {
            var toast = new bootstrap.Toast(document.getElementById("adviceOne"))

            toast.show()
            oneTriggered = true;
        }
        else if (currentPostition > popoverPositions[1] && !twoTriggered) {
            var toast = new bootstrap.Toast(document.getElementById("adviceTwo"))

            toast.show()
            twoTriggered = true;
        }
        else if (currentPostition > popoverPositions[2] && !threeTriggered) {
            var toast = new bootstrap.Toast(document.getElementById("adviceThree"))

            toast.show()
            threeTriggered = true;
        }

    });
})

