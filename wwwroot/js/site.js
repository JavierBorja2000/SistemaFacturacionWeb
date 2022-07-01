// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const $totalFactura = document.querySelector("#totalFactura")

document.addEventListener("DOMContentLoaded", () => {

    document.addEventListener("click", e => {
        if (e.target.matches(".btn-disminuir")) {
            const $cantidad = e.target.nextElementSibling

            if ($cantidad.textContent === "0") return

            let cantidadActual = parseInt($cantidad.textContent) - 1
            $cantidad.textContent = cantidadActual

            //Actualizaciones
            let precio = parseFloat(e.target.parentElement.previousElementSibling.textContent)
            const $totalPorProducto = e.target.parentElement.nextElementSibling.querySelector("#totalPorProducto")

            actualizarTotalPorProducto(cantidadActual, precio, $totalPorProducto)

            actualizarTotalFactura()
        }

        if (e.target.matches(".btn-aumentar")) {

            const cantidad = e.target.previousElementSibling

            let cantidadActual = parseInt(cantidad.textContent) + 1

            //Verificar existencias
            let existencias = e.target.closest("tr").querySelector("#existencias").textContent
            if (cantidadActual > parseInt(existencias)) return 

            cantidad.textContent = cantidadActual

            //Actualizaciones
            let precio = parseFloat(e.target.parentElement.previousElementSibling.textContent)
            const $totalPorProducto = e.target.parentElement.nextElementSibling.querySelector("#totalPorProducto")

            actualizarTotalPorProducto(cantidadActual, precio, $totalPorProducto)

            actualizarTotalFactura()
        }
    })
})

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