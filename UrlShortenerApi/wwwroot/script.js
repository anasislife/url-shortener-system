let generatedLink = "";

async function shortenUrl() {
    const urlInput = document.getElementById("urlInput");
    const shortUrlOutput = document.getElementById("shortUrlOutput");
    const messageBox = document.getElementById("messageBox");

    const originalUrl = urlInput.value.trim();

    if (!originalUrl) {
        messageBox.innerHTML = `<span class="error">الرجاء إدخال رابط أولاً</span>`;
        shortUrlOutput.value = "";
        generatedLink = "";
        return;
    }

    try {
        const response = await fetch("/api/ShortUrls", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ originalUrl: originalUrl })
        });

        const data = await response.json();

        if (!response.ok) {
            messageBox.innerHTML = `<span class="error">فشل في إنشاء الرابط المختصر</span>`;
            shortUrlOutput.value = "";
            generatedLink = "";
            return;
        }

        generatedLink = data.shortUrl.replace("https://localhost:7258/", "https://localhost:7258/go/");
        shortUrlOutput.value = generatedLink;

        messageBox.innerHTML = `<span class="success">تم إنشاء الرابط المختصر بنجاح</span>`;
    } catch (error) {
        messageBox.innerHTML = `<span class="error">تعذر الاتصال بالسيرفر</span>`;
        shortUrlOutput.value = "";
        generatedLink = "";
    }
}

function copyGeneratedLink() {
    const shortUrlOutput = document.getElementById("shortUrlOutput");
    const messageBox = document.getElementById("messageBox");

    if (!shortUrlOutput.value.trim()) {
        messageBox.innerHTML = `<span class="error">لا يوجد رابط لنسخه</span>`;
        return;
    }

    navigator.clipboard.writeText(shortUrlOutput.value).then(() => {
        messageBox.innerHTML = `<span class="success">تم نسخ الرابط</span>`;
    }).catch(() => {
        messageBox.innerHTML = `<span class="error">فشل النسخ</span>`;
    });
}