// DisasterEye — site.js
document.addEventListener('DOMContentLoaded', function () {
    // Auto-dismiss alerts após 5s
    document.querySelectorAll('.de-alert').forEach(function (el) {
        setTimeout(function () {
            el.style.transition = 'opacity .5s';
            el.style.opacity = '0';
            setTimeout(() => el.remove(), 500);
        }, 5000);
    });
});
