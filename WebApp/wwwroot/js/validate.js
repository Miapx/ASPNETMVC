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

//ChatGPT för att fetcha projectid på edit-knappen
//document.querySelectorAll(".edit-project-btn").forEach(btn => {
//    btn.addEventListener("click", async () => {
//        const projectId = btn.dataset.projectId;
//        console.log(projectId)
//        const response = await fetch(`/Projects/EditProject?id=${projectId}`);
//        const html = await response.text();
        //const modal = document.getElementById('editform-container');

        //modal.innerHTML = html;

        //const targetModal = document.getElementById('editProjectModal')
        //if (targetModal) {
        //    targetModal.style.display = "flex";
        //}
//    });
//});


//document.querySelectorAll(".edit-project-btn").forEach(btn => {
//    btn.addEventListener("click", async () => {
//        const projectId = btn.dataset.projectId;
//        const response = await fetch(`/Projects/EditProject?id=${projectId}`);
//        const html = await response.text();

//        const modal = document.querySelector('#editProjectModal');
//        if (modal) {
//            modal.innerHTML = html;
//            modal.style.display = "flex";

//            // Lägg till close-listener på den nya knappen som finns i den laddade partialen
//            const closeBtn = modal.querySelector('[data-close="true"]');
//            if (closeBtn) {
//                closeBtn.addEventListener('click', () => {
//                    modal.style.display = 'none';
//                });
//            }
//        }
//    });
//});

