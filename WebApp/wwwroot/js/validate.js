const validateField = (field) => {
    let errorSpan = document.querySelector(`span[data-valmsg-for='${field.name}'`)
    if (!errorSpan) return;

    let errorMessage = ""
    let value = field.value.trim()

    if (field.hasAttribute("data-val-required") && value === "")
        errorMessage = field.getAttribute("data-val-required")

    if (field.hasAttribute("data-val-regex") && value !== "") {
        let pattern = new RegExp(field.getAttribute("data-val-regex-pattern"))
        if (!pattern.test(value))
        errorMessage = field.getAttribute("data-val-regex")
    }

    if (errorMessage) {
        //tar bort klassen för giltigt ifyllt fält
        errorSpan.classList.remove("field-validation-valid")
        //lägger på klassen för ogiltigt ifyllt fält
        errorSpan.classList.add("text-danger")
        errorSpan.textContent = errorMessage

    } else {
        errorSpan.classList.remove("text-danger")
        errorSpan.classList.add("field-validation-valid")
        errorSpan.textContent = ""
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form")

    if (!form) return;

    const fields = form.querySelectorAll("input[data-val='true']")

    fields.forEach(field => {
        field.addEventListener("input", function () {
            validateField(field)
        })
    })
})