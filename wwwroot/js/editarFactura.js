// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const $totalFactura = document.querySelector("#totalFactura")

document.addEventListener("DOMContentLoaded", () => {
    calcularTotalInicial()

    document.addEventListener("click", e => {
        if (e.target.matches(".btn-disminuir")) {
            const $cantidad = e.target.nextElementSibling
            console.log($cantidad)

            if ($cantidad.value === "0") return

            let cantidadActual = parseInt($cantidad.value) - 1
            $cantidad.value = cantidadActual

            //Actualizaciones
            let precio = parseFloat(e.target.parentElement.previousElementSibling.textContent)
            const $totalPorProducto = e.target.parentElement.nextElementSibling.querySelector("#totalPorProducto")

            actualizarTotalPorProducto(cantidadActual, precio, $totalPorProducto)

            actualizarTotalFactura()
        }

        if (e.target.matches(".btn-aumentar")) {

            const cantidad = e.target.previousElementSibling

            let cantidadActual = parseInt(cantidad.value) + 1

            //Verificar existencias
            let existencias = e.target.closest("tr").querySelector("#existencias").textContent
            if (cantidadActual > parseInt(existencias)) return 

            cantidad.value = cantidadActual

            //Actualizaciones
            let precio = parseFloat(e.target.parentElement.previousElementSibling.textContent)
            const $totalPorProducto = e.target.parentElement.nextElementSibling.querySelector("#totalPorProducto")

            actualizarTotalPorProducto(cantidadActual, precio, $totalPorProducto)

            actualizarTotalFactura()
        }

        if (e.target.matches(".cambiarCliente") || e.target.matches(".clienteSeleccionado")) {

            const $listadoClientes = document.querySelector("#selectNuevoCliente")
            $listadoClientes.classList.toggle("ocultar")
        }

        if(e.target.matches("#estado")){
            var anulada_input = document.querySelector("#estado_input");
            var anulada_span = document.querySelector("#estado_span");
            console.log(anulada_span);

            if(anulada_input.value === "A"){
                anulada_input.value = "N";
                anulada_span.textContent = "Estado: Vigente";
            }
            else{
                anulada_input.value = "A";
                anulada_span.textContent = "Estado: Anulada";
            }
        }

        if (e.target.matches("#clienteSeleccionado")) {
            const $fila = e.target.closest("tr")
            const $inputCodigoCliente = document.querySelector("#cod_cliente_input");
            console.log($fila)
            //Cambiar el codigo de cliente del cliente
            console.log($fila.querySelector("#cod_cliente"))
            $inputCodigoCliente.value = $fila.querySelector("#cod_cliente").textContent

            //cambiar el nombre del cliente en el resumen de factura
            let nombreNuevoCliente = $fila.querySelector("#nombres").textContent
            let apellidoNuevoCliente = $fila.querySelector("#apellidos").textContent
            document.querySelector("#nombre_cliente").textContent = `Emitida por: ${nombreNuevoCliente} ${apellidoNuevoCliente}`

            //cambiar el nit del cliente en el resumen de factura
            let nitNuevoCliente = $fila.querySelector("#nit").textContent
            document.querySelector("#nit_cliente").textContent = `NIT: ${nitNuevoCliente}`
        }
    })
})

function calcularTotalInicial() {
    const $listaProductos = document.querySelectorAll("#tableProductos tbody tr")

    $listaProductos.forEach(el => {
        let cantidad = el.querySelector("#cantidad").value
        let precio = el.querySelector("#precio").textContent
        const $totalPorProducto = el.querySelector("#totalPorProducto")

        actualizarTotalPorProducto(parseInt(cantidad), parseFloat(precio), $totalPorProducto)

        actualizarTotalFactura()
    })
}

function actualizarTotalPorProducto(cantidad, precio, total) {

    if (cantidad === 0) total.textContent = "0.00"

    if (cantidad !== 0) {
        total.textContent = (cantidad * precio).toFixed(2)
    }
}

function actualizarTotalFactura() {
    const totalesDeProductos = document.querySelectorAll("#totalPorProducto")
    let totalesDeProductosInt = []

    totalesDeProductos.forEach(el => {
        totalesDeProductosInt.push(parseFloat(el.textContent))
    })

    let total = totalesDeProductosInt.reduce((accum, num) => accum + num , 0)

    if (total === 0) {
        $totalFactura.textContent = "0.00"
    } else {
        $totalFactura.textContent = total.toFixed(2)
    }
}

function cambiarCliente(Codigo_cliente){
    var cod_cliente_input = document.querySelector("#cod_cliente_input");
    cod_cliente_input.value = Codigo_cliente;
}