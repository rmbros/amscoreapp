$('.allowNumbersOnly').keyup(function (e) {
    if (/\D/g.test(this.value)) {
        this.value = this.value.replace(/\D/g, '');
    }
});
