// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



/**
 * 
 * @param {number} questionId
 * @param {HTMLFormElement} form
 */
async function postNewComment(questionId, form) {
    let formData = new FormData(form);
    formData.append("questionId", questionId);
    //serialize the form
    formData = new URLSearchParams(formData).toString();
    //submit form
    let response = await sendPostRequest(form.action, formData).catch(function (error) {
        if (error.message.includes("An error occurred")) {
            //custom error thrown inside the function.
            //showAlert(error.message.replace("Error: ", ""));
            alert(error.message.replace("Error: ", ""));
        } else {
            //showAlert("A connection error occurred. please check your network and try again.");
            alert("A connection error occurred. please check your network and try again.");
        }
    });

    return response;
}


async function sendGetRequest(url) {
    var myInit = {
        method: 'GET',
        mode: 'same-origin',
        cache: 'default'
    };

    let response = await fetch(url, myInit);
    if (!response.ok) {
        if (response.statusText.length > 0) {
            throw new Error(`An error occurred: ${response.statusText}`);
        } else {
            throw new Error(`An error occurred, error code: ${response.status}`);
        }
    } else {
        return await response.json(); // parses response into native JavaScript objects
    }
}

async function sendPostRequest(url, formdata, returnType = "json") {
    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        //headers: {
        //    //'Content-Type': 'application/json'
        //    'Content-Type': 'application/x-www-form-urlencoded'
        //},
        redirect: 'follow', // manual, *follow, error
        body: formdata // body data type must match "Content-Type" header
    });

    if (!response.ok) {
        if (response.statusText.length > 0) {
            throw new Error(`An error occurred: ${response.statusText}`);
        } else {
            throw new Error(`An error occurred, error code: ${response.status}`);
        }
    } else {
        return await response.json(); // parses response into native JavaScript objects
    }
}
