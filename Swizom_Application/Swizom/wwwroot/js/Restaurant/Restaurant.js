function openDeleteModal(id, name, address, contact)
{
    document.getElementById('restaurantId').value = id;
    document.getElementById('restaurantName').innerText = name;
    document.getElementById('restaurantAddress').innerText = address;
    document.getElementById('restaurantContact').innerText = contact;

    var modal = new bootstrap.Modal(document.getElementById('deleteModal'));
    modal.show();
}