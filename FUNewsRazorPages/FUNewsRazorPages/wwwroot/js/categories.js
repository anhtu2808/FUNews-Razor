$(document).ready(function() {
    // Edit Modal
    $('#editCategoryModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var categoryId = button.data('category-id');
        var categoryName = button.data('category-name');
        var categoryDescription = button.data('category-description');
        var parentCategoryId = button.data('parent-category-id');
        var isActive = button.data('is-active');
        
        // Populate form fields
        $('#editCategoryId').val(categoryId);
        $('#editCategoryName').val(categoryName);
        $('#editCategoryDescription').val(categoryDescription);
        $('#editParentCategoryId').val(parentCategoryId || '');
        
        // Handle checkbox
        $('#editIsActive').prop('checked', isActive === true || isActive === 'True');
        
        // Remove current category from parent options to prevent circular reference
        $('#editParentCategoryId option[value="' + categoryId + '"]').prop('disabled', true);
    });

    // Reset parent category options when modal is hidden
    $('#editCategoryModal').on('hidden.bs.modal', function () {
        $('#editParentCategoryId option').prop('disabled', false);
    });

    // Create Modal
    $('#createCategoryModal').on('show.bs.modal', function (event) {
        // Clear form fields
        $('#createCategoryName').val('');
        $('#createCategoryDescription').val('');
        $('#createParentCategoryId').val('');
        $('#createIsActive').prop('checked', true);
        
        // Clear validation messages
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').text('');
    });

    // Delete Modal
    $('#deleteCategoryModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var categoryId = button.data('category-id');
        var categoryName = button.data('category-name');
        var hasChildren = button.data('has-children');
        
        // Populate delete info
        $('#deleteCategoryName').text(categoryName);
        
        // Remove existing hidden input if any
        $('#deleteCategoryForm input[name="id"]').remove();
        // Add hidden input for id
        $('#deleteCategoryForm').append('<input type="hidden" name="id" value="' + categoryId + '" />');
        
        // Show/hide warning message and disable delete button if has children
        if (hasChildren === true || hasChildren === 'True') {
            $('#deleteWarningMessage').removeClass('d-none');
            $('#confirmDeleteBtn').prop('disabled', true);
        } else {
            $('#deleteWarningMessage').addClass('d-none');
            $('#confirmDeleteBtn').prop('disabled', false);
        }
    });

    // Form validation
    function validateForm(formId) {
        var isValid = true;
        var form = $(formId);
        
        // Clear previous validation
        form.find('.is-invalid').removeClass('is-invalid');
        form.find('.invalid-feedback').text('');
        
        // Validate category name
        var categoryName = form.find('input[name="Category.CategoryName"]').val().trim();
        if (!categoryName) {
            form.find('input[name="Category.CategoryName"]').addClass('is-invalid');
            form.find('input[name="Category.CategoryName"]').siblings('.invalid-feedback').text('Category name is required.');
            isValid = false;
        } else if (categoryName.length > 100) {
            form.find('input[name="Category.CategoryName"]').addClass('is-invalid');
            form.find('input[name="Category.CategoryName"]').siblings('.invalid-feedback').text('Category name cannot exceed 100 characters.');
            isValid = false;
        }
        
        // Validate description length
        var description = form.find('textarea[name="Category.CategoryDescription"]').val();
        if (description && description.length > 500) {
            form.find('textarea[name="Category.CategoryDescription"]').addClass('is-invalid');
            form.find('textarea[name="Category.CategoryDescription"]').siblings('.invalid-feedback').text('Description cannot exceed 500 characters.');
            isValid = false;
        }
        
        return isValid;
    }

    // Handle form submissions with validation
    $('#createCategoryForm').on('submit', function(e) {
        if (!validateForm('#createCategoryForm')) {
            e.preventDefault();
        }
    });

    $('#editCategoryForm').on('submit', function(e) {
        if (!validateForm('#editCategoryForm')) {
            e.preventDefault();
        }
    });

    // Character count for description fields
    function setupCharacterCount(textareaId, maxLength) {
        var textarea = $(textareaId);
        var countDisplay = $('<div class="form-text text-end"><small><span class="char-count">0</span>/' + maxLength + ' characters</small></div>');
        textarea.after(countDisplay);
        
        textarea.on('input', function() {
            var currentLength = $(this).val().length;
            countDisplay.find('.char-count').text(currentLength);
            
            if (currentLength > maxLength) {
                countDisplay.addClass('text-danger');
            } else if (currentLength > maxLength * 0.8) {
                countDisplay.removeClass('text-danger').addClass('text-warning');
            } else {
                countDisplay.removeClass('text-danger text-warning');
            }
        });
    }

    // Setup character counters
    setupCharacterCount('#createCategoryDescription', 500);
    setupCharacterCount('#editCategoryDescription', 500);
}); 