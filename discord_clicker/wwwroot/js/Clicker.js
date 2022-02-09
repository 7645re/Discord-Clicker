let button = document.getElementsByClassName("clicker_button")[0]
let htmlCounter = document.getElementsByClassName("clicker_counter")[0]
let content = document.getElementById("content")
let cpsCounter = document.getElementsByClassName("cps_counter")[0]
let baseUrl = document.location.origin 
let click = 0
let userCoefficient;
let userPassiveCoefficient;
var salary = 0


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
            let user = await getUser()
            let userClickCoefficient = user["clickCoefficient"]
            let userPassiveCoefficient = user["passiveCoefficient"]
            let result = await setMoneyDB(userClickCoefficient * click + 4 * userPassiveCoefficient)
            if (result["result"] == "ok") {
                if (userPassiveCoefficient >= 100) {
                    salary = salary + userCoefficient
                    let iter = userCoefficient/4000*10
                    console.log(salary)
                    console.log(iter)
                    setTimeout(function tick() {
                      if (salary >= 0) {
                          htmlCounter.innerText = Number(htmlCounter.innerText)+iter
                          salary = salary-iter
                        setTimeout(tick, 10);
                    }
                  }, 10);
                }
                else {
                    setMoneySite(Number(getMoneySite()) + 4 * userPassiveCoefficient)
                }
                let ImgUp = '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-alarm-fill" viewBox="0 0 16 16"><path d="M6 .5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 0 1H9v1.07a7.001 7.001 0 0 1 3.274 12.474l.601.602a.5.5 0 0 1-.707.708l-.746-.746A6.97 6.97 0 0 1 8 16a6.97 6.97 0 0 1-3.422-.892l-.746.746a.5.5 0 0 1-.707-.708l.602-.602A7.001 7.001 0 0 1 7 2.07V1h-.5A.5.5 0 0 1 6 .5zm2.5 5a.5.5 0 0 0-1 0v3.362l-1.429 2.38a.5.5 0 1 0 .858.515l1.5-2.5A.5.5 0 0 0 8.5 9V5.5zM.86 5.387A2.5 2.5 0 1 1 4.387 1.86 8.035 8.035 0 0 0 .86 5.387zM11.613 1.86a2.5 2.5 0 1 1 3.527 3.527 8.035 8.035 0 0 0-3.527-3.527z"/></svg>'
                let ImgDown = '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-hand-index-thumb-fill" viewBox="0 0 16 16"><path d="M8.5 1.75v2.716l.047-.002c.312-.012.742-.016 1.051.046.28.056.543.18.738.288.273.152.456.385.56.642l.132-.012c.312-.024.794-.038 1.158.108.37.148.689.487.88.716.075.09.141.175.195.248h.582a2 2 0 0 1 1.99 2.199l-.272 2.715a3.5 3.5 0 0 1-.444 1.389l-1.395 2.441A1.5 1.5 0 0 1 12.42 16H6.118a1.5 1.5 0 0 1-1.342-.83l-1.215-2.43L1.07 8.589a1.517 1.517 0 0 1 2.373-1.852L5 8.293V1.75a1.75 1.75 0 0 1 3.5 0z"/></svg>'
                click = 0
                let alert = createAlert(ImgUp, "AutoSave", ImgDown, result["money"] == undefined ? result["reason"] : result["money"])
                document.body.appendChild(alert)
                alertDropDown(alert)
            }
            else {
                click = 0
                console.log(result["reason"])
            }
            cpsCounter.innerHTML = userPassiveCoefficient + " cps"
            
    }, interval);
}

async function reloadPageBackUp() {
    let user = await getUser()
    userCoefficient = user["clickCoefficient"]
    userPassiveCoefficient = user["passiveCoefficient"]
    let moneyDb = user["money"]
    setMoneySite(moneyDb)
    cpsCounter.innerHTML = userPassiveCoefficient + " cps"
    return user
}


function ActionClick() {
    click++
    if (userCoefficient >= 100) {
        salary = salary + userCoefficient
        let iter = userCoefficient/1000*10
        console.log(salary)
        console.log(iter)
        setTimeout(function tick() {
          if (salary >= 0) {
              htmlCounter.innerText = Number(htmlCounter.innerText)+iter
              salary = salary-iter
            setTimeout(tick, 10);
        }
      }, 10);
    }
    else {
        setMoneySite(parseInt(getMoneySite()) + userCoefficient)
    }
}