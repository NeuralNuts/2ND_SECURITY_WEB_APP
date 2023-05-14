var productArray = [];

$.ajax({
    type: "GET",
    url: 'https://localhost:7080/api/Product/GetProducts',
    dataType: "JSON",
    success: function (response) {
        productArray = response;
        buildProductTabel(productArray);
        console.log(productArray);
    }
})

function buildProductTabel(data) {
    var table = document.getElementById('my-table');

    for (var i = 0; i < data.length; i++) {

        var row = `<tr>
                       <td>${data[i].security_plan}</td>
                       <td>${data[i].subscription}</td>
                       <td>$${data[i].price}</td>
                       <td><button class="btn btn-primary">edit</button></td>
                       <td><button class="btn btn-primary">delete</button></td>
                   </tr>`
        table.innerHTML += row;
    }
}