$(document).ready(function () {
	$("#thumbs-up").click(function () {
		$("#thumbs-up").toggleClass("active");
		$("#thumbs-down").removeClass("active");
	});

	$("#thumbs-down").click(function () {
		$("#thumbs-down").toggleClass("active");
		$("#thumbs-up").removeClass("active");
	});
});