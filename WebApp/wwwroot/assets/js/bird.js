var Bird = Backbone.View.extend({

    tagName: "div",
  
    className: "bird",
  
  
  
    initialize: function() {
      console.log("Bird.init() ");
      
      //_.bind(this.remove, this);
      
      this.$el.addClass( 'bird-' + Math.round(Math.random()*2 + 1));
      
    },
  
    render: function() {
        console.log("Bird.render()");
        var that = this;
      
          var screenWidth = $(window).width() - 150;
          var yDistance = $(window).height() - 200;
          this.$el.css({ top: Math.random()* yDistance + 'px', left: '-30px' });
          
      
        TweenMax.to( this.$el, 12, {
          bezier:[
            //{left:screenWidth/3, top: Math.random()* yDistance},
            //{left:screenWidth/3*2, top: Math.random()* yDistance}, 
            //{left: screenWidth + 100, top: Math.random()* yDistance}
            {left:Math.random() * screenWidth*0.75, top: Math.random()* yDistance},
            {left:Math.random() * screenWidth*0.75, top: Math.random()* yDistance}, 
            {left: screenWidth + 100, top: Math.random()* yDistance}
            ],
          ease:Power1.easeInOut,
          onComplete: function(){
            that.remove();
          }
          //delay: Math.random() * 5
        });
        
        return this;
    }
  
  });
  
  var numberOfBirds = 10,
      interval = 5000;
  

  
  function addBirds(){
    for(var i=0; i < numberOfBirds; i++){
      
      _.delay(function(){
        var bird = new Bird();
        //console.log(bird);
        $('body').append(bird.$el);
        bird.render();
      }, Math.random() * 5000);
  
    }
  }
