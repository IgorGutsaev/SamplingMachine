window.ui = {
    objects: [],
    activateTooltips () {
        $('[data-toggle="tooltip"]').tooltip();
        $('a[data-toggle="tooltip"]').tooltip({
            animated: 'fade',
            placement: 'bottom',
            html: true
        });
    },
    hideModalManually(modalId) {
        $(`#${modalId} .close`).trigger("click");
    }
}