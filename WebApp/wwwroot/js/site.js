document.addEventListener('DOMContentLoaded', () => {

    //open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]')

    modalButtons.forEach(button => {
        button.addEventListener('click', async () => {

            console.log('CLICK: Edit/Add modal button clicked')

            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)
            const mode = button.getAttribute('data-mode') // "edit" eller "add"

            if (modal) {
                if (mode === "edit") {
                    const projectId = button.getAttribute('data-project-id')
                    if (projectId) {
                        const response = await fetch(`/Projects/GetProject/${projectId}`)
                        const data = await response.json()
                        console.log(data)


                        modal.querySelector('[name="ProjectId"]').value = data.projectId
                        modal.querySelector('[name="ProjectName"]').value = data.projectName
                        modal.querySelector('[name="ClientName"]').value = data.clientName
                        modal.querySelector('[name="Description"]').value = data.description
                        modal.querySelector('[name="StartDate"]').value = data.startDate?.substring(0, 10) || ""
                        modal.querySelector('[name="EndDate"]').value = data.endDate?.substring(0, 10) || ""
                        modal.querySelector('[name="Budget"]').value = data.budget ?? ""
                    }
                }



                // Visa modalen
                modal.style.display = 'flex'
            }
        })
    })

    //close big modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal')

            if (modal) {

                
                modal.style.display = 'none'

                //clear formdata
            }
        })
    })

    //open small modal (user logout)

    //var openButton = document.querySelector('[data-target="#userInfoModal"]');
    //openButton.addEventListener('click', function () {
    //    smallModal.style.display = "block";
    //});



    //close small modal(user logout)
    //var moreModal = document.getElementById("moreOption");
    var smallModal = document.getElementById("userInfoModal");

    window.onclick = function (e) {
        if (e.target == smallModal) {
            smallModal.style.display = "none";
        }
        //if (e.target == moreModal) {
        //    moreModal.style.display = "none";
        //}
    }

    //var moreModal = document.getElementById("moreOption");

    //window.onclick = function (e) {
    //    if (e.target == moreModal) {
    //        moreModal.style.display = "none";
    //    }
    //}




})


//More-knappen som dropdown

const dropdowns = document.querySelectorAll('[data-type="dropdown"]')

document.addEventListener('click', function (event) {
    let clickedDropdown = null

    dropdowns.forEach(dropdown => {
        const targetId = dropdown.getAttribute('data-target')
        const targetElement = document.querySelector(targetId)

        if (dropdown.contains(event.target)) {
            clickedDropdown = targetElement

            document.querySelectorAll('.show').forEach(openDropdown => {
                if (openDropdown !== targetElement) {
                    openDropdown.classList.remove('show')
                }
            })

            targetElement.classList.toggle('show')

        }
    })

    if (!clickedDropdown && !event.target.closest('.show')) {
        document.querySelectorAll('.show').forEach(openDropdown => {
            openDropdown.classList.remove('show')
        })
    }

})




    //handle submit forms
    const forms = document.querySelectorAll('form')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            clearErrorMessages(form)

            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('.modal')
                    if (modal)
                        modal.style.display = 'none';

                        window.location.reload()
                }

                else if (res.status === 400) {
                    const data = await res.json()

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            let input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            let span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error', 'text-danger')
                            }
                        })
                    }
                }
            }
            catch {
                console.log('error submitting form')
            }
        })
    })

    function clearErrorMessages(form) {
        form.querySelectorAll('[data-val="true"]').forEach(input => {
            input.classList.remove('input-validation-error')
        })

        form.querySelectorAll('[data-valmsg-for').forEach(span => {
            span.innerText = ''
            span.classList.remove('field-validation-error', 'text-danger')
        })
    }


