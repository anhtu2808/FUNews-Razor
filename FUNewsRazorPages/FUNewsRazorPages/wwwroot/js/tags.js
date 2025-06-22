$(document).ready(function() {
    // Edit Modal
    $('#editTagModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var tagId = button.data('tag-id');
        var tagName = button.data('tag-name');
        var tagNote = button.data('tag-note');
        
        // Populate form fields
        $('#editTagId').val(tagId);
        $('#editTagName').val(tagName);
        $('#editTagNote').val(tagNote);
    });

    // Create Modal
    $('#createTagModal').on('show.bs.modal', function (event) {
        // Clear form fields
        $('#createTagName').val('');
        $('#createTagNote').val('');
    });

    // Delete Modal
    $('#deleteTagModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var tagId = button.data('tag-id');
        var tagName = button.data('tag-name');
        
        // Populate delete info and add hidden input for id
        $('#deleteTagName').text(tagName);
        
        // Remove existing hidden input if any
        $('#deleteTagForm input[name="id"]').remove();
        // Add hidden input for id
        $('#deleteTagForm').append('<input type="hidden" name="id" value="' + tagId + '" />');
    });
}); 