setTimeout(function tick2() {
        buffer += userPassiveCoefficient
    setTimeout(tick2, 1000);
}, 1000);

setTimeout(function tick() {
    let iter = buffer/1000*60
    if (buffer != 0) {
        htmlCounter.innerText = (Number(htmlCounter.innerText)+iter).toFixed(0)
        buffer -= iter
        dBuffer += iter
    }
    setTimeout(tick, 40);
}, 40);

function getMoneySite() {
    return htmlCounter.innerText
}
function setMoneySite(money) {
    htmlCounter.innerText = money
    return money
}