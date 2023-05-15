var productArray = [];

$("#add-product").click(function (event) {

    $.ajax({
        type: "POST",
        url: "https://localhost:7080/api/Product/PostProduct?" + `security_plan=${$("#security-plan-input").val()}&subscription=${$("#subscription-plan-input").val()}&price=${$("#price-input").val()}`,
        dataType: "JSON",
        success: function (response) {
            console.log(response)
        }
    })

    location.reload()
})

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
    var prodcut_id_input = document.getElementById("product-id-input")
    var security_input = document.getElementById("security-plan-input")
    var subscription_input = document.getElementById("subscription-plan-input")
    var price_input = document.getElementById("price-input")
    var table = document.getElementById('my-table');
    var true_table = document.getElementById("true-table")
    var cells = table.getElementsByTagName('td');

    for (var i = 0; i < data.length; i++) {

        //<td><button class="btn btn-primary">edit</button></td>
        //<td><button class="btn btn-primary">delete</button></td>

        var row = `<tr data-bs-toggle="modal" data-bs-target="#myModal2">
                       <td>${data[i].product_id}</td>
                       <td>${data[i].security_plan}</td>
                       <td>${data[i].subscription}</td>
                       <td>$${data[i].price}</td>
                   </tr>`
        table.innerHTML += row;
    }

    for (var i = 0; i < cells.length; i++) {
        var cell = cells[i];
        cell.onclick = function () {
            var rowId = this.parentNode.rowIndex;
            var rowsNotSelected = true_table.getElementsByTagName('tr');

            for (var row = 0; row < rowsNotSelected.length; row++) {
                rowsNotSelected[row].style.backgroundColor = "";
                rowsNotSelected[row].classList.remove('selected');
            }

            var rowSelected = true_table.getElementsByTagName('tr')[rowId];

            $("#product-id-input").val(rowSelected.cells[0].innerHTML)
            $("#security-plan-input2").val(rowSelected.cells[1].innerHTML)
            $("#subscription-plan-input").val(rowSelected.cells[2].innerHTML)
            $("#price-input").val(rowSelected.cells[3].innerHTML)

            console.log(rowSelected.cells[3].innerHTML)
        }
    }
}