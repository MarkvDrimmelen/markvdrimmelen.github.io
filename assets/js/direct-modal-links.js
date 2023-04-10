$(document).ready(function () {

    let dictionary = new Object();
    dictionary["#collapsed"] = "#collapsedModal";
    dictionary["#clothingCuller"] = "#clothingCullerModal";
    dictionary["#teamObstacleCourse"] = "#teamObstacleCourseModal";

    for (const key of Object.keys(dictionary)) {
        if (window.location.href.indexOf(key) != -1) {
            var modal = $(dictionary[key]);

            modal.on('hidden.bs.modal', () => {
                window.location.href = '../#projects';
            })

            modal.modal('show');
        }
    }
});