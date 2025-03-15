const emailFilter = document.getElementById('emailFilter');

emailFilter.addEventListener('input', function () {
    const filter = this.value.toLowerCase();
    const rows = document.querySelectorAll('tbody tr');

    rows.forEach(function (row) {
        const emailCell = row.querySelector('td:nth-child(3)');
        if (emailCell) {
            const email = emailCell.textContent.toLowerCase();
            row.style.display = email.includes(filter) ? '' : 'none';
        }
    });
});

emailFilter.addEventListener('keydown', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
        const inputEvent = new Event('input');
        this.dispatchEvent(inputEvent);
    }
});