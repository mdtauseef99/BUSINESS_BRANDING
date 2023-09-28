$(function(){
	formMiddle();
	$(window).resize(function(){
		formMiddle();
	});

	$('.corporate-bxslider').bxSlider({
		slideWidth: 740,
		minSlides: 1,
		maxSlides: 1,
		slideMargin: 10
	});
	$('.connected').bxSlider({
		slideWidth: 240,
		minSlides: 1,
		maxSlides: 3,
		slideMargin: 10
	});
	$('.footer-bxslider').bxSlider({
	    slideWidth: 250,
	    slideheight: 250,
	    minSlides: 1,
	    maxSlides: 5,
	    slideMargin: 10
	});	

	$('.profile-bxslider').bxSlider({
	    slideWidth: 280,
	    minSlides: 1,
	    maxSlides: 5,
	    slideMargin: 10
	});

	$('.slider4').bxSlider({
	    slideWidth: 300,
	    minSlides: 2,
	    maxSlides: 3,
	    moveSlides: 1,
	    slideMargin: 10
	});

	$('.products-bxslider').bxSlider({
	    slideWidth: 240,
	    minSlides: 1,
	    maxSlides: 5,
	    slideMargin: 10
	});
	

	
	$('[data-toggle=tooltip]').tooltip();
});
function formMiddle(){
	var w = $('.carousel-inner').width()/2, El = $('.form').width()/2;
	$('.form.home').css('marginLeft',w-El + 'px');
}
 
$('.mainBanner ul').bxSlider({
    auto: true,
	pager: false,
	controls:false
});
$('.homBannerRight ul').bxSlider({
    auto: false
});
$('.homBannerLeft ul').bxSlider({
    auto: true,
	mode: 'fade',
	onSlideAfter: function(e){
		e.find('div').addClass('show');
	},
	onSlideBefore: function(e){
		e.find('div').removeClass('show');
	}
});