document.addEventListener("DOMContentLoaded", function () {
    initializeTimeago();
    initializeEventListeners();
    displaySuccessMessage();
});

function initializeTimeago() {
    timeago.render(document.querySelectorAll('.timeago'));
}

function initializeEventListeners() {
    document.getElementById('selectAll').addEventListener('change', handleSelectAllChange);

    document.getElementById('blockButton').addEventListener('click', function () {
        updateUsers('Block');
    });

    document.getElementById('unblockButton').addEventListener('click', function () {
        updateUsers('Unblock');
    });

    document.getElementById('deleteButton').addEventListener('click', function () {
        updateUsers('Delete');
    });
}

function handleSelectAllChange() {
    var checkboxes = document.querySelectorAll('.userCheckbox');
    for (var checkbox of checkboxes) {
        checkbox.checked = this.checked;
    }
}

function updateUsers(action) {
    var selectedUserIds = getSelectedUserIds();

    if (selectedUserIds.length === 0) {
        showErrorMessage();
        return;
    } else {
        hideErrorMessage();
    }

    sendUserUpdateRequest(action, selectedUserIds);
}

function getSelectedUserIds() {
    return Array.from(document.querySelectorAll('.userCheckbox:checked')).map(cb => cb.value);
}

function showErrorMessage() {
    var errorMessage = document.getElementById('errorMessage');
    var successMessage = document.getElementById('successMessage');
    successMessage.style.display = 'none';
    errorMessage.style.display = 'block';
}

function hideErrorMessage() {
    var errorMessage = document.getElementById('errorMessage');
    errorMessage.style.display = 'none';
}

function sendUserUpdateRequest(action, userIds) {
    fetch('/User/' + action, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(userIds)
    })
        .then(response => {
            if (response.redirected) {
                window.location.href = response.url;
            } else {
                return response.json();
            }
        })
        .then(data => {
            if (data) {
                handleUpdateSuccess(action);
            }
        })
}

function handleUpdateSuccess(action) {
    let actionText = '';
    switch (action) {
        case 'Block':
            actionText = 'blocked';
            break;
        case 'Unblock':
            actionText = 'unblocked';
            break;
        case 'Delete':
            actionText = 'deleted';
            break;
    }
    localStorage.setItem('successMessage', `Users successfully ${actionText}.`);
    location.reload();
}

function displaySuccessMessage() {
    var successMessageText = localStorage.getItem('successMessage');
    if (successMessageText) {
        var successMessageElement = document.getElementById('successMessage');
        successMessageElement.textContent = successMessageText;
        successMessageElement.style.display = 'block';
        localStorage.removeItem('successMessage');
        setTimeout(() => {
            successMessageElement.style.display = 'none';
        }, 3000);
    }
}