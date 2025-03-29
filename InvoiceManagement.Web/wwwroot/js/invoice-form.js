$(document).ready(function () {
    // Add new item
    $('#addItem').click(function () {
        var index = $('.item-row').length;
        var newItem = `
            <div class="item-row row mb-3 g-3">
                <div class="col-md-5">
                    <input name="Invoice.Items[${index}].Description" class="form-control" required />
                </div>
                <div class="col-md-2">
                    <input name="Invoice.Items[${index}].Quantity" class="form-control quantity" type="number" min="1" value="1" required />
                </div>
                <div class="col-md-2">
                    <div class="input-group">
                        <span class="input-group-text">$</span>
                        <input name="Invoice.Items[${index}].UnitPrice" class="form-control unit-price" type="number" min="0.01" step="0.01" value="0.01" required />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="input-group">
                        <span class="input-group-text">$</span>
                        <input class="form-control amount" readonly value="0.01" />
                    </div>
                </div>
                <div class="col-md-1 d-flex align-items-end">
                    <button type="button" class="btn btn-danger remove-item">
                        <i class="fas fa-trash-alt"></i>
                    </button>
                </div>
            </div>
        `;
        $('#itemsContainer').append(newItem);
        calculateTotal();
    });

    // Remove item
    $(document).on('click', '.remove-item', function () {
        $(this).closest('.item-row').remove();
        reindexItems();
        calculateTotal();
    });

    // Calculate amount when quantity or price changes
    $(document).on('input', '.quantity, .unit-price', function () {
        var row = $(this).closest('.item-row');
        var quantity = parseFloat(row.find('.quantity').val()) || 0;
        var unitPrice = parseFloat(row.find('.unit-price').val()) || 0;
        var amount = quantity * unitPrice;
        row.find('.amount').val(amount.toFixed(2));
        calculateTotal();
    });

    // Reindex items for model binding
    function reindexItems() {
        $('.item-row').each(function (index) {
            $(this).find('input[name*="Description"]').attr('name', `Invoice.Items[${index}].Description`);
            $(this).find('input[name*="Quantity"]').attr('name', `Invoice.Items[${index}].Quantity`);
            $(this).find('input[name*="UnitPrice"]').attr('name', `Invoice.Items[${index}].UnitPrice`);
        });
    }

    // Calculate total amount
    function calculateTotal() {
        var total = 0;
        $('.amount').each(function () {
            total += parseFloat($(this).val()) || 0;
        });
        $('#totalAmount').text('$' + total.toFixed(2));
    }

    // Initial calculation
    calculateTotal();

    // Form validation
    (function () {
        'use strict';
        var form = document.getElementById('invoiceForm');
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    })();
});