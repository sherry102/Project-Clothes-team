$(function () {
    $("#menu-women, #menu-men, #menu-kids").hide();
    $(".menu-women, #menu-women").on("mouseenter", function () {
      $("#menu-women").stop().slideDown();
      $(".menu-women").css({
        "border-bottom": "1px solid #ddd",
      });
    });
    $(".menu-women, #menu-women").on("mouseleave", function () {
      $("#menu-women").stop().slideUp();
      $(".menu-women").css({
        "border-bottom": "none",
      });
    });
    $(".menu-men, #menu-men").on("mouseenter", function () {
      $("#menu-men").stop().slideDown();
      $(".menu-men").css({
        "border-bottom": "1px solid #ddd",
      });
    });
    $(".menu-men, #menu-men").on("mouseleave", function () {
      $("#menu-men").stop().slideUp();
      $(".menu-men").css({
        "border-bottom": "none",
      });
    });
    $(".menu-kids, #menu-kids").on("mouseenter", function () {
      $("#menu-kids").stop().slideDown();
      $(".menu-kids").css({
        "border-bottom": "1px solid #ddd",
      });
    });
    $(".menu-kids, #menu-kids").on("mouseleave", function () {
      $("#menu-kids").stop().slideUp();
      $(".menu-kids").css({
        "border-bottom": "none",
      });
    });
});