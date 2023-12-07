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
    },
    htmlizePopover() {
        const list = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
        list.map((el) => {
            let opts = {
                animation: false,
            }
            if (el.hasAttribute('data-bs-content-id')) {
                opts.content = document.getElementById(el.getAttribute('data-bs-content-id')).innerHTML;
                opts.html = true;
            }
            new bootstrap.Popover(el, opts);
        })
    },
    hidePopover() {
        const elements = document.getElementsByClassName("popover");
        while (elements.length > 0) {
            elements[0].parentNode.removeChild(elements[0]);
        }
    },
    redirect(url) {
        window.location.href = 'url';
    }
}