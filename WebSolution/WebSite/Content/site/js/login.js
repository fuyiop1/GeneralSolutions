$(document).ready(function(){
	$('#RegTrigger').click(function(){
		$('#LogForm').hide();
		$(this).next('#RegForm').slideToggle();
		$('#LogTrigger').removeClass('active');
		$(this).toggleClass('active');					
		
		if ($(this).hasClass('active')) $(this).find('span')
			else $(this).find('span')
		});
});

$(document).ready(function(){
	$('#LogTrigger').click(function(){
		$('#RegForm').hide();
		$(this).next('#LogForm').slideToggle();
		$('#RegTrigger').removeClass('active');
		$(this).toggleClass('active');				
		
		if ($(this).hasClass('active')) $(this).find('span')
			else $(this).find('span')
		});
});