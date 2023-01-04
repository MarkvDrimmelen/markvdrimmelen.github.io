var wow = new WOW();
wow.init();

$(document).ready(function(){
    $("#skills-tab").click(function(){
        $("div[class*='progress-bar']").each(function ()
        {
            wow.show(this);
        });
    });
});