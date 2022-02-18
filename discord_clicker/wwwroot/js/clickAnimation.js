const random = (min, max) => Math.floor(Math.random() * (max - min)) + min;
button.addEventListener("click", ActionClick, false)
let animationOpen = true
async function imgBounce() {
    let end = false
    let barrierFlag = true
    let growthFlag = false
    let stSizeWd = button.width
    let stSizeWdMax = stSizeWd+50
    let stSizeWdMin = stSizeWd-50
    setTimeout(function scale() {
        stSizeWd = growthFlag ? stSizeWd+=10 : stSizeWd-=10
        if (stSizeWd == stSizeWdMin && barrierFlag && !end) {
            barrierFlag = false
            growthFlag = true
        }
        if (stSizeWd == stSizeWdMax) {
            end = true
            animationOpen = true
        }
        if (!end) {
            button.width = stSizeWd
            setTimeout(scale, 10)
        }
    }, 10)
}


async function clickIncrease() {
    let text = document.createElement("span")
    let rndH = random(40, 60)
    let rndW = random(40, 60)
    text.textContent = userClickCoefficient
    text.style.position = "absolute"
    text.style.top = rndH+"%"
    text.style.left = rndW+"%"
    text.style.fontSize = "500%"
    text.style.pointerEvents='none';
    content.appendChild(text)
    setTimeout(function upmove() {
        if (Number(text.style.top.replace("%", "")) > 0) {
            text.style.top = rndH-- + "%"
            text.style.opacity = rndH-- + "%"
            setTimeout(upmove, 10)
        } 
        else {
            text.remove()
        }
    }, 10)
}

/** Action function for each user click on the main button */
async function ActionClick() {
    let lUserMoney = localStorage.getItem('money');
    counterFloat+=Number(localStorage.getItem("clickCoefficient"))
    localStorage.setItem('money', Number(lUserMoney === null ? 0 : lUserMoney) + Number(localStorage.getItem("clickCoefficient")));
    clickIncrease()
    if (animationOpen) {
        imgBounce()
        animationOpen = false
    }
}