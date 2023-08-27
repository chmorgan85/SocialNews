const spinner = document.createElement('span');
spinner.classList.add('spinner-border');

// Enable Bootstrap popovers
const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl));

// "Share" buttons
const shareBtns = document.querySelectorAll('.shareBtn');
shareBtns.forEach((btn) => {
    btn.addEventListener('click', (event) => {
        event.preventDefault();

        const url = new URL(btn.getAttribute('href'), document.baseURI).href;
        navigator.clipboard.writeText(url);
    });
});

// Post voting
function postUpvote(event) {
    event.preventDefault();

    const form = event.target;
    const submitBtn = form.querySelector('button');
    const voteCountDiv = form.querySelector('div');
    const voteCount = Number(voteCountDiv.textContent);

    // Show loading spinner
    voteCountDiv.textContent = '';
    voteCountDiv.appendChild(spinner);

    fetch(form.getAttribute('action'), {
        method: 'post',
        body: new FormData(form)
    })
        .then((response) => {
            voteCountDiv.removeChild(spinner);

            if (response.redirected) {
                const loginURL = new URL('/Identity/Account/Login', document.baseURI).href;
                window.location.href = loginURL;
            }
            else if (response.ok) {
                voteCountDiv.textContent = voteCount + 1;
                submitBtn.classList.remove('btn-outline-success');
                submitBtn.classList.add('btn-success');

                form.setAttribute('action', '/Posts/RemoveUpvote');
                form.classList.remove('postUpvote');
                form.classList.add('postRemoveUpvote');

                form.removeEventListener('submit', postUpvote);
                form.addEventListener('submit', postRemoveUpvote);
            }
            else {
                voteCountDiv.textContent = voteCount;
                alert(response.status);
            }
        });
}

function postRemoveUpvote(event) {
    event.preventDefault();

    const form = event.target;
    const submitBtn = form.querySelector('button');
    const voteCountDiv = form.querySelector('div');
    const voteCount = Number(voteCountDiv.textContent);

    // Show loading spinner
    voteCountDiv.textContent = '';
    voteCountDiv.appendChild(spinner);

    fetch(form.getAttribute('action'), {
        method: 'post',
        body: new FormData(form)
    })
        .then((response) => {
            voteCountDiv.removeChild(spinner);

            if (response.redirected) {
                const loginURL = new URL('/Identity/Account/Login', document.baseURI).href;
                window.location.href = loginURL;
            }
            else if (response.ok) {
                voteCountDiv.textContent = voteCount - 1;
                submitBtn.classList.remove('btn-success');
                submitBtn.classList.add('btn-outline-success');

                form.setAttribute('action', '/Posts/Upvote');
                form.classList.remove('postRemoveUpvote');
                form.classList.add('postUpvote');

                form.removeEventListener('submit', postRemoveUpvote);
                form.addEventListener('submit', postUpvote);
            }
            else {
                voteCountDiv.textContent = voteCount;
                alert(response.status);
            }
        });
}

const postUpvoteForms = document.querySelectorAll('.postUpvote');
postUpvoteForms.forEach((form) => {
    form.addEventListener('submit', postUpvote);
});

const postRemoveUpvoteForms = document.querySelectorAll('.postRemoveUpvote');
postRemoveUpvoteForms.forEach((form) => {
    form.addEventListener('submit', postRemoveUpvote);
});


// Post saving/unsaving
function savePost(event) {
    event.preventDefault();

    const form = event.target;
    const submitBtn = form.querySelector('button');
    const submitBtnSpan = form.querySelector('span');

    // Show loading spinner
    submitBtnSpan.textContent = '';
    submitBtnSpan.appendChild(spinner);

    fetch(form.getAttribute('action'), {
        method: 'post',
        body: new FormData(form)
    })
        .then((response) => {
            submitBtnSpan.removeChild(spinner);

            if (response.redirected) {
                const loginURL = new URL('/Identity/Account/Login', document.baseURI).href;
                window.location.href = loginURL;
            }
            else if (response.ok) {
                submitBtnSpan.textContent = 'Unsave';
                submitBtn.classList.remove('btn-primary');
                submitBtn.classList.add('btn-danger');

                form.setAttribute('action', '/Posts/Unsave');
                form.classList.remove('postSave');
                form.classList.add('postUnsave');

                form.removeEventListener('submit', savePost);
                form.addEventListener('submit', unsavePost);
            }
            else {
                submitBtnSpan.textContent = 'Save';
                alert(response.status);
            }
        });
}

function unsavePost(event) {
    event.preventDefault();

    const form = event.target;
    const submitBtn = form.querySelector('button');
    const submitBtnSpan = form.querySelector('span');

    // Show loading spinner
    submitBtnSpan.textContent = '';
    submitBtnSpan.appendChild(spinner);

    fetch(form.getAttribute('action'), {
        method: 'post',
        body: new FormData(form)
    })
        .then((response) => {
            submitBtnSpan.removeChild(spinner);

            if (response.redirected) {
                const loginURL = new URL('/Identity/Account/Login', document.baseURI).href;
                window.location.href = loginURL;
            }
            else if (response.ok) {
                submitBtnSpan.textContent = 'Save';
                submitBtn.classList.remove('btn-danger');
                submitBtn.classList.add('btn-primary');

                form.setAttribute('action', '/Posts/Save');
                form.classList.remove('postUnsave');
                form.classList.add('postSave');

                form.removeEventListener('submit', unsavePost);
                form.addEventListener('submit', savePost);
            }
            else {
                submitBtnSpan.textContent = 'Unsave';
                alert(response.status);
            }
        });
}

const postSaveForms = document.querySelectorAll('.postSave');
postSaveForms.forEach((form) => {
    form.addEventListener('submit', savePost);
});

const postUnsaveForms = document.querySelectorAll('.postUnsave');
postUnsaveForms.forEach((form) => {
    form.addEventListener('submit', unsavePost);
});


// Comment voting
function commentUpvote(event) {
    event.preventDefault();

    const form = event.target;
    const submitBtn = form.querySelector('button');
    const voteCountDiv = form.querySelector('div');
    const voteCount = Number(voteCountDiv.textContent);

    // Show loading spinner
    voteCountDiv.textContent = '';
    voteCountDiv.appendChild(spinner);

    fetch(form.getAttribute('action'), {
        method: 'post',
        body: new FormData(form)
    })
        .then((response) => {
            voteCountDiv.removeChild(spinner);

            if (response.redirected) {
                const loginURL = new URL('/Identity/Account/Login', document.baseURI).href;
                window.location.href = loginURL;
            }
            else if (response.ok) {
                voteCountDiv.textContent = voteCount + 1;
                submitBtn.classList.remove('btn-outline-success');
                submitBtn.classList.add('btn-success');

                form.setAttribute('action', '/Comments/RemoveUpvote');
                form.classList.remove('commentUpvote');
                form.classList.add('commentRemoveUpvote');

                form.removeEventListener('submit', commentUpvote);
                form.addEventListener('submit', commentRemoveUpvote);
            }
            else {
                voteCountDiv.textContent = voteCount;
                alert(response.status);
            }
        });
}

function commentRemoveUpvote(event) {
    event.preventDefault();

    const form = event.target;
    const submitBtn = form.querySelector('button');
    const voteCountDiv = form.querySelector('div');
    const voteCount = Number(voteCountDiv.textContent);

    // Show loading spinner
    voteCountDiv.textContent = '';
    voteCountDiv.appendChild(spinner);

    fetch(form.getAttribute('action'), {
        method: 'post',
        body: new FormData(form)
    })
        .then((response) => {
            voteCountDiv.removeChild(spinner);

            if (response.redirected) {
                const loginURL = new URL('/Identity/Account/Login', document.baseURI).href;
                window.location.href = loginURL;
            }
            else if (response.ok) {
                voteCountDiv.textContent = voteCount - 1;
                submitBtn.classList.remove('btn-success');
                submitBtn.classList.add('btn-outline-success');

                form.setAttribute('action', '/Comments/Upvote');
                form.classList.remove('commentRemoveUpvote');
                form.classList.add('commentUpvote');

                form.removeEventListener('submit', commentRemoveUpvote);
                form.addEventListener('submit', commentUpvote);
            }
            else {
                voteCountDiv.textContent = voteCount;
                alert(response.status);
            }
        });
}

const commentUpvoteForms = document.querySelectorAll('.commentUpvote');
commentUpvoteForms.forEach((form) => {
    form.addEventListener('submit', commentUpvote);
});

const commentRemoveUpvoteForms = document.querySelectorAll('.commentRemoveUpvote');
commentRemoveUpvoteForms.forEach((form) => {
    form.addEventListener('submit', commentRemoveUpvote);
});


// Comment "Edit" buttons
const commentEditBtns = document.querySelectorAll('.commentEditBtn');
commentEditBtns.forEach((btn) => {
    btn.addEventListener('click', (event) => {
        const editForm = btn.parentElement.previousElementSibling;
        const contentDiv = editForm.previousElementSibling;
        contentDiv.setAttribute('hidden', '');
        editForm.removeAttribute('hidden');
    });
});

// Comment edit reset buttons
const commentEditForms = document.querySelectorAll('.commentEditForm');
commentEditForms.forEach((form) => {
    form.addEventListener('reset', (event) => {
        const contentDiv = form.previousElementSibling;
        contentDiv.removeAttribute('hidden');
        form.setAttribute('hidden', '');
    });
});
