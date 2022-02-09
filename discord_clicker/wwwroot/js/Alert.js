function createAlert(ImgUp, TextUp, ImgDown, TextDown) {
    let result = document.createElement("div")
    result.innerHTML = `<div class="alert" role="alert">
    <div class="alert-up">
    ${ImgUp} ${TextUp}
    </div>
    <div class="alert-down">
    ${ImgDown}
    ${TextDown}
    </div>
    </div>` 
    result.style.position = "absolute"
    result.style.top = window.innerHeight + "px"
    result.style.zIndex = "100"
    return result
}

function alertDropDown(elm) {

    let num = -10
    var refreshIntervalId = setInterval(function () {
        if (num <= 10) {
            if (-(num ** 2) + 100 > 80) {
                elm.style.top = window.innerHeight - 80 + "px"
                num += 0.1
            }
            else {
                elm.style.top = window.innerHeight - (-(num ** 2) + 100) + "px"
                num += 0.1
            }
        }
        else {
            elm.remove()
            clearInterval(refreshIntervalId)
        }
    }, 5);
}