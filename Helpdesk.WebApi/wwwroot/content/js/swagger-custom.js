(function (){
    window.addEventListener('load', (event) => {
        const oldLink = document.querySelector("link[rel*='icon']") || document.createElement('link');
        document.head.removeChild(oldLink);

        const linkIcon = document.createElement('link');
        linkIcon.rel = 'icon';
        linkIcon.type = 'image/x-icon';
        linkIcon.rel = 'shortcut icon';
        linkIcon.href = '/content/images/favicon.png';
        linkIcon.sizes = '16x16';
        document.querySelector('head').appendChild(linkIcon);
    });
})();