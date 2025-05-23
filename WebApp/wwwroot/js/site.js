﻿document.addEventListener('DOMContentLoaded', () => {

    // Open modal
    document.addEventListener('click', async (event) => {
        const button = event.target.closest('[data-modal="true"]');
        if (!button) return;

        console.log('CLICK: Edit/Add modal button clicked');

        const modalTarget = button.getAttribute('data-target');
        const modal = document.querySelector(modalTarget);
        const mode = button.getAttribute('data-mode');

        console.log("Modal found, mode is:", mode);

        if (modal) {
            if (mode === "edit") {
                const projectId = button.getAttribute('data-project-id');
                if (projectId) {
                    try {
                        const response = await fetch(`/Projects/GetProject/${projectId}`);
                        const data = await response.json();
                        console.log(data);

                        modal.querySelector('[name="ProjectId"]').value = data.projectId || "";
                        modal.querySelector('[name="ProjectName"]').value = data.projectName || "";
                        modal.querySelector('[name="ClientName"]').value = data.clientName || "";
                        editProjectDescriptionEditor.root.innerHTML = data.description || "";
                        modal.querySelector('[name="StartDate"]').value = data.startDate?.substring(0, 10) || "";
                        modal.querySelector('[name="EndDate"]').value = data.endDate?.substring(0, 10) || "";
                        modal.querySelector('[name="Budget"]').value = data.budget ?? "";
                    } catch (err) {
                        console.log('Error fetching project:', err);
                    }
                }
            }

            modal.style.display = 'flex';
        }
    });

    // Close modal
    document.addEventListener('click', (event) => {
        const button = event.target.closest('[data-close="true"]');
        if (!button) return;

        const modal = button.closest('.modal');
        if (modal) {
            modal.style.display = 'none';
        }
    });

    // Close User Info Modal 
    const smallModal = document.getElementById("userInfoModal");
    window.addEventListener('click', function (e) {
        if (e.target === smallModal) {
            smallModal.style.display = "none";
        }
    });

    // Dropdowns
    const dropdowns = document.querySelectorAll('[data-type="dropdown"]');
    document.addEventListener('click', function (event) {
        let clickedDropdown = null;

        dropdowns.forEach(dropdown => {
            const targetId = dropdown.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);

            if (dropdown.contains(event.target)) {
                clickedDropdown = targetElement;

                document.querySelectorAll('.show').forEach(openDropdown => {
                    if (openDropdown !== targetElement) {
                        openDropdown.classList.remove('show');
                    }
                });

                targetElement.classList.toggle('show');
            }
        });

        if (!clickedDropdown && !event.target.closest('.show')) {
            document.querySelectorAll('.show').forEach(openDropdown => {
                openDropdown.classList.remove('show');
            });
        }
    });

    // DeleteProject
    document.addEventListener('click', async (event) => {
        const button = event.target.closest('.delete-btn');
        if (!button) return;

        const projectId = button.getAttribute('data-project-id');
        const confirmed = confirm("Är du säker på att du vill radera projektet?");
        if (confirmed) {
            try {
                const response = await fetch(`/Projects/DeleteProject/${projectId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
                });
                if (response.ok) {
                    alert("Projektet raderades!");
                    location.reload();
                } else {
                    alert("Något gick fel vid raderingen.");
                }
            } catch (err) {
                console.log('Error deleting project:', err);
            }
        }
    });

    // WYSIWYG AddProject
    const addProjectDescriptionTextArea = document.getElementById('add-project-description');
    const addProjectDescriptionQuill = new Quill('#add-project-description-wysiwyg-editor', {
        modules: { syntax: true, toolbar: '#add-project-description-wysiwyg-toolbar' },
        theme: 'snow',
        placeholder: 'Type something'
    });
    addProjectDescriptionQuill.on('text-change', function () {
        addProjectDescriptionTextArea.value = addProjectDescriptionQuill.root.innerHTML;
    });

    // WYSIWYG EditProject
    const editProjectDescriptionEditor = new Quill('#edit-project-description-wysiwyg-editor', {
        theme: 'snow',
        modules: { toolbar: '#edit-project-description-wysiwyg-toolbar' }
    });


    //ChatGPT to limit lines and characters in desciption field
    const MAX_DESCRIPTION_LINES = 3;

    editProjectDescriptionEditor.on('text-change', function (delta, oldDelta, source) {
        const lines = editProjectDescriptionEditor.getLines();

        if (lines.length > MAX_DESCRIPTION_LINES) {
            const lastLine = lines[MAX_DESCRIPTION_LINES];
            const lastLineIndex = editProjectDescriptionEditor.getIndex(lastLine);
            editProjectDescriptionEditor.deleteText(lastLineIndex - 1, lastLine.length() + 1);
        }
    });

    editProjectDescriptionEditor.on('text-change', function (delta, oldDelta, source) {
        const text = editProjectDescriptionEditor.getText();

        if (text.trim().length > MAX_DESCRIPTION_LENGTH) {
            editProjectDescriptionEditor.deleteText(MAX_DESCRIPTION_LENGTH, text.length);
        }
    });

    const editProjectForm = document.querySelector('#editProjectModal form');
    editProjectForm.addEventListener('submit', function (e) {
        const hiddenInput = document.querySelector('#edit-project-description');
        hiddenInput.value = editProjectDescriptionEditor.root.innerHTML;
    });

    addProjectDescriptionQuill.on('text-change', function (delta, oldDelta, source) {
        const lines = addProjectDescriptionQuill.getLines();

        if (lines.length > MAX_DESCRIPTION_LINES) {
            const lastLine = lines[MAX_DESCRIPTION_LINES];
            const lastLineIndex = addProjectDescriptionQuill.getIndex(lastLine);
            addProjectDescriptionQuill.deleteText(lastLineIndex - 1, lastLine.length() + 1);
        }

        addProjectDescriptionTextArea.value = addProjectDescriptionQuill.root.innerHTML;
    });


    const MAX_DESCRIPTION_LENGTH = 100;

    addProjectDescriptionQuill.on('text-change', function (delta, oldDelta, source) {
        const text = addProjectDescriptionQuill.getText();

        if (text.trim().length > MAX_DESCRIPTION_LENGTH) {
            addProjectDescriptionQuill.deleteText(MAX_DESCRIPTION_LENGTH, text.length);
        }

        addProjectDescriptionTextArea.value = addProjectDescriptionQuill.root.innerHTML;
    });
    //End of ChatGPT

    // Forms
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            clearErrorMessages(form);

            const formData = new FormData(form);
            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                });

                if (res.ok) {
                    const modal = form.closest('.modal');
                    if (modal) modal.style.display = 'none';
                    window.location.reload();
                } else if (res.status === 400) {
                    const data = await res.json();
                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`);
                            if (input) input.classList.add('input-validation-error');

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`);
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error', 'text-danger');
                            }
                        });
                    }
                }
            } catch (err) {
                console.log('Error submitting form:', err);
            }
        });
    });

    function clearErrorMessages(form) {
        form.querySelectorAll('[data-val="true"]').forEach(input => {
            input.classList.remove('input-validation-error');
        });

        form.querySelectorAll('[data-valmsg-for]').forEach(span => {
            span.innerText = '';
            span.classList.remove('field-validation-error', 'text-danger');
        });
    }
});