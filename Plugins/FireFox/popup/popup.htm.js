//@---------Ershad Raoufi Developer------

//--------------Rest Api Link------------
const _url = 'http://localhost:5200/api/v1';

//--------Magic Function :)----------
function CallShortLinkApi() {

    //--------------Fetch data from Input tags-----------
    let _orginalUrl = document.getElementById('OrginalUrl').value;
    let _shortUrl = document.getElementById('ShortUrl').value;
    let _pass = document.getElementById('Password').value;
    //---------Check Orginal Input tag----------
    if (_orginalUrl)
        _orginalUrl = "http://www." + _orginalUrl;
    else {
        document.getElementById("error").textContent = "لطفا لینک بلند را وارد کنید";
        return;
    }
    //---------Send Data and Fetch data from Server-----------
    fetch(_url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ OrginalUrl: _orginalUrl, ShortUrl : _shortUrl, Pass: _pass })
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}
//-------Start of Magic-----------
document.addEventListener("click", (e) => {
    if (e.target.classList.contains("btn-success")) {
        CallShortLinkApi();
    }
});
