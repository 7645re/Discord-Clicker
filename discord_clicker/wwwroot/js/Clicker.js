let button = document.getElementsByClassName("clicker_button")[0]
let htmlCounter = document.getElementsByClassName("clicker_counter")[0]
let content = document.getElementById("content")
let baseUrl = document.location.origin 
let click = 0
let userCoefficient;
var all_click = 0


reloadPageBackUp()
intervalAutoSave(4000)
button.addEventListener("click", ActionClick, false)


async function asyncRequest(method, url, bode = null) {
    return fetch(url).then(response => {
        return response.json()
    })
}


function getMoneySite() {
    return htmlCounter.innerText
}
function setMoneySite(money) {
    htmlCounter.innerText = money
    return money
}

async function setMoneyDB(money) {
    let result;
    await asyncRequest('GET', `setmoney?money=${money}`)
        .then(data => { result = data })
        .catch(err => console.log(err))
    return result

}
async function getUser() {
    let result;
    await asyncRequest('GET', "/getuserinformation")
        .then(data => { result = data["user"] })
        .catch(err => console.log(err))
    return result

}


function intervalAutoSave(interval) {
    setInterval(async () => {
        if (click > 0) {
            let user = await getUser()
            let userCoefficient = user["clickCoefficient"]
            let result = await setMoneyDB(userCoefficient * click)
            click = 0
            let alert = document.createElement("div")
            alert.innerHTML = `<div class="alert alert-success d-flex align-items-center" role="alert">
                          <div>
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-cloud-upload-fill" viewBox="0 0 16 16">
                              <path fill-rule="evenodd" d="M8 0a5.53 5.53 0 0 0-3.594 1.342c-.766.66-1.321 1.52-1.464 2.383C1.266 4.095 0 5.555 0 7.318 0 9.366 1.708 11 3.781 11H7.5V5.707L5.354 7.854a.5.5 0 1 1-.708-.708l3-3a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1-.708.708L8.5 5.707V11h4.188C14.502 11 16 9.57 16 7.773c0-1.636-1.242-2.969-2.834-3.194C12.923 1.999 10.69 0 8 0zm-.5 14.5V11h1v3.5a.5.5 0 0 1-1 0z"/>
                            </svg>
                            AutoSave <img width="20" src="https://pnggrid.com/wp-content/uploads/2021/05/Black-and-white-Discord-Logo-1024x784.png"/>
                                ${result["money"]}
                          </div>
                        </div>`
            alert.style.width = "100%"
            alert.style.position = "absolute"
            alert.style.top = window.innerHeight + "px"
            alertDropDown(alert)
            content.appendChild(alert)
        }
    }, interval);
}

async function reloadPageBackUp() {
    let user = await getUser()
    userCoefficient = user["clickCoefficient"]
    let moneyDb = user["money"]
    setMoneySite(moneyDb)
    return user
}




$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
});

function ActionClick() {
    click++
    setMoneySite(parseInt(getMoneySite()) + userCoefficient)

}
function alertDropDown(elm) {

    let num = -10
    var refreshIntervalId = setInterval(function () {
        if (num <= 10) {
            if (-(num ** 2) + 100 > 80) {
                elm.style.top = window.innerHeight - 80 - 50 + "px"
                num += 0.1
            }
            else {
                elm.style.top = window.innerHeight - (-(num ** 2) + 100) - 50 + "px"
                num += 0.1
            }
        }
        else {
            elm.remove()
            clearInterval(refreshIntervalId)
        }
    }, 5);
}