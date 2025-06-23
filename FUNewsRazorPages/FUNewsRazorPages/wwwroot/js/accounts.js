// Account Management JavaScript

$(document).ready(function() {
    console.log('Account Management JS loaded');
    
    // Edit Modal
    $('#editAccountModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var accountId = button.data('account-id');
        var accountName = button.data('account-name');
        var accountEmail = button.data('account-email');
        var accountRole = button.data('account-role');
        
        console.log('Edit modal opening for account:', accountId);
        
        // Populate form fields
        $('#editAccountId').val(accountId);
        $('#editAccountName').val(accountName);
        $('#editAccountEmail').val(accountEmail);
        $('#editAccountRole').val(accountRole);
    });

    // Create Modal
    $('#createAccountModal').on('show.bs.modal', function (event) {
        // Clear form fields
        $('#createAccountName').val('');
        $('#createAccountEmail').val('');
        $('#createAccountRole').val('');
        $('#createAccountPassword').val('');
    });

    // Delete Modal
    $('#deleteAccountModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var accountId = button.data('account-id');
        var accountName = button.data('account-name');
        var accountEmail = button.data('account-email');
        
        console.log('Delete modal opening for account:', accountId);
        
        // Populate delete info
        $('#deleteAccountId').val(accountId);
        $('#deleteAccountName').text(accountName);
        $('#deleteAccountEmail').text(accountEmail);
    });

    // Character counters
    function setupCharacterCounter(inputId, maxLength) {
        const input = document.getElementById(inputId);
        if (input) {
            const counter = input.parentNode.querySelector('.char-counter .current-chars');
            if (counter) {
                input.addEventListener('input', function() {
                    counter.textContent = this.value.length;
                });
            }
        }
    }

    // Setup character counters
    setupCharacterCounter('createAccountName', 100);
    setupCharacterCounter('createAccountEmail', 70);
    setupCharacterCounter('createAccountPassword', 70);
    setupCharacterCounter('editAccountName', 100);
    setupCharacterCounter('editAccountEmail', 70);
    setupCharacterCounter('editAccountPassword', 70);
});

function initializeAccountManagement() {
    // Initialize modals
    initializeEditModal();
    initializeDeleteModal();
    initializeCreateModal();
    
    // Initialize character counters
    initializeCharacterCounters();
    
    // Initialize form validation
    initializeFormValidation();
}

// Edit Modal Functions
function initializeEditModal() {
    const editModal = document.getElementById('editAccountModal');
    if (!editModal) return;
    
    editModal.addEventListener('show.bs.modal', function(event) {
        const button = event.relatedTarget;
        
        const accountId = button.getAttribute('data-account-id');
        const accountName = button.getAttribute('data-account-name');
        const accountEmail = button.getAttribute('data-account-email');
        const accountRole = button.getAttribute('data-account-role');
        
        // Fill the form
        document.getElementById('editAccountId').value = accountId;
        document.getElementById('editAccountName').value = accountName;
        document.getElementById('editAccountEmail').value = accountEmail;
        document.getElementById('editAccountRole').value = accountRole;
        
        // Clear password field
        document.getElementById('editAccountPassword').value = '';
        
        // Update character counters
        updateCharacterCounter(document.getElementById('editAccountName'));
        updateCharacterCounter(document.getElementById('editAccountEmail'));
        updateCharacterCounter(document.getElementById('editAccountPassword'));
        
        // Update modal title
        document.getElementById('editAccountModalLabel').innerHTML = 
            '<i class="bi bi-pencil-square me-2"></i>Edit Account: ' + accountName;
    });
    
    // Clear form when modal is hidden
    editModal.addEventListener('hidden.bs.modal', function() {
        clearForm('editAccountModal');
    });
}

// Delete Modal Functions
function initializeDeleteModal() {
    const deleteModal = document.getElementById('deleteAccountModal');
    if (!deleteModal) return;
    
    deleteModal.addEventListener('show.bs.modal', function(event) {
        const button = event.relatedTarget;
        
        const accountId = button.getAttribute('data-account-id');
        const accountName = button.getAttribute('data-account-name');
        const accountEmail = button.getAttribute('data-account-email');
        
        // Set the account ID
        document.getElementById('deleteAccountId').value = accountId;
        
        // Update account info in modal
        document.getElementById('deleteAccountName').textContent = accountName;
        document.getElementById('deleteAccountEmail').textContent = accountEmail;
    });
}

// Create Modal Functions
function initializeCreateModal() {
    const createModal = document.getElementById('createAccountModal');
    if (!createModal) return;
    
    // Clear form when modal is hidden
    createModal.addEventListener('hidden.bs.modal', function() {
        clearForm('createAccountModal');
    });
}

// Character Counter Functions
function initializeCharacterCounters() {
    const inputs = document.querySelectorAll('input[maxlength], textarea[maxlength]');
    
    inputs.forEach(input => {
        // Update counter on input
        input.addEventListener('input', function() {
            updateCharacterCounter(this);
        });
        
        // Initialize counter
        updateCharacterCounter(input);
    });
}

function updateCharacterCounter(input) {
    const maxLength = input.getAttribute('maxlength');
    if (!maxLength) return;
    
    const currentLength = input.value.length;
    const counter = input.parentNode.querySelector('.char-counter .current-chars');
    
    if (counter) {
        counter.textContent = currentLength;
        
        const parentCounter = counter.parentNode;
        parentCounter.classList.remove('warning', 'danger');
        
        if (currentLength > maxLength * 0.9) {
            parentCounter.classList.add('danger');
        } else if (currentLength > maxLength * 0.7) {
            parentCounter.classList.add('warning');
        }
    }
}

// Form Validation Functions
function initializeFormValidation() {
    // Real-time validation for email fields
    const emailInputs = document.querySelectorAll('input[type="email"]');
    emailInputs.forEach(input => {
        input.addEventListener('blur', function() {
            validateEmail(this);
        });
    });
    
    // Real-time validation for required fields
    const requiredInputs = document.querySelectorAll('input[required], select[required]');
    requiredInputs.forEach(input => {
        input.addEventListener('blur', function() {
            validateRequired(this);
        });
    });
}

function validateEmail(input) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const isValid = emailRegex.test(input.value);
    
    input.classList.remove('is-valid', 'is-invalid');
    
    if (input.value.length > 0) {
        if (isValid) {
            input.classList.add('is-valid');
        } else {
            input.classList.add('is-invalid');
        }
    }
    
    return isValid;
}

function validateRequired(input) {
    const isValid = input.value.trim().length > 0;
    
    input.classList.remove('is-valid', 'is-invalid');
    
    if (isValid) {
        input.classList.add('is-valid');
    } else {
        input.classList.add('is-invalid');
    }
    
    return isValid;
}

// Utility Functions
function clearForm(modalId) {
    const modal = document.getElementById(modalId);
    if (!modal) return;
    
    // Clear all form inputs
    const inputs = modal.querySelectorAll('input:not([type="hidden"]), select, textarea');
    inputs.forEach(input => {
        if (input.type === 'checkbox' || input.type === 'radio') {
            input.checked = false;
        } else {
            input.value = '';
        }
        
        // Remove validation classes
        input.classList.remove('is-valid', 'is-invalid');
        
        // Update character counters
        updateCharacterCounter(input);
    });
    
    // Clear validation messages
    const validationMessages = modal.querySelectorAll('.text-danger');
    validationMessages.forEach(msg => {
        msg.textContent = '';
    });
}

// Role Helper Functions
function getRoleName(roleId) {
    const roles = {
        1: 'Staff',
        2: 'Lecturer',
        3: 'Admin'
    };
    return roles[roleId] || 'Unknown';
}

function getRoleClass(roleId) {
    return `role-${roleId}`;
}

// Toast notification function (if needed)
function showToast(message, type = 'success') {
    // Create toast element
    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${type} border-0`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    
    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;
    
    // Add to toast container
    let toastContainer = document.querySelector('.toast-container');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.className = 'toast-container position-fixed bottom-0 end-0 p-3';
        document.body.appendChild(toastContainer);
    }
    
    toastContainer.appendChild(toast);
    
    // Show toast
    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();
    
    // Remove toast after it's hidden
    toast.addEventListener('hidden.bs.toast', function() {
        toast.remove();
    });
} 