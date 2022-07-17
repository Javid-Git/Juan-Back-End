$(document).ready(function () {
    $(document).on('click', '.addreply', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.replyContainer').append(data)
            })
    })
    $(document).on('click', '.modaldetail', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');

        console.log(url);

        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            $('.modal-content').html(data);
            $('.quick-view-image').slick({
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: false,
                dots: false,
                fade: true,
                asNavFor: '.quick-view-thumb',
                speed: 400,
            });

            $('.quick-view-thumb').slick({
                slidesToShow: 4,
                slidesToScroll: 1,
                asNavFor: '.quick-view-image',
                dots: false,
                arrows: false,
                focusOnSelect: true,
                speed: 400,
            });
            
        })
    })
    $(document).on('click', '.sortlink', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.sort-content').html(data);
            });
    })
    //$('.sortlink').click(function (e) {
    //    e.preventDefault();
    //    let url = $(this).attr('href');

    //    console.log(url);

    //    fetch(url).then(response => {
    //        return response.text();
    //    }).then(data => {
    //        $('.sort-content').html(data);
    //    })
    //})
    $('.searchinput').keyup(function () {
        console.log("eee");
        let value = $(this).val();
        console.log(value);

        let url = $(this).data("url");
        url = url + '?search=' + value.trim();
        console.log(url);

        if (value) {
            fetch(url)
                .then(response => response.text())

                .then(data => {
                    $('.search-body .list-group').html(data)

                })
        }
        else {
            $('.search-body .list-group').html('')
        }

    })
   
    $(document).on('click', '.addtobasket', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        console.log(url)
        fetch(url)
            .then(res => res.json())
            .then(data => {
                $('.notification').html(data);
            });
    })
    $(document).on('click', '.openbasket', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        console.log(url)
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.header-cart').html(data);
            });
    })
    $(document).on('click', '.deletefrombasket', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.header-cart').html(data);
                fetch('/basket/DeleteUpdate')
                    .then(res => res.json())
                    .then(data => {
                        $('.notification').html(data);
                    });
            })
    })
    $(document).on('click', '.subCount', function (e) {
        e.preventDefault();
        let inputCount = $(this).next().val();

        if (inputCount >= 2) {
            inputCount--;
            $(this).next().val(inputCount);
            let url = $(this).attr('href') + '/?count=' + inputCount;
            console.log('sub');
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $('.basketindexcontainer').html(data);
                    fetch('/basket/getbasket')
                        .then(res => res.text())
                        .then(data => {
                            $('.header-cart').html(data);
                        });
                });
        }
    })

    $(document).on('click', '.addCount', function (e) {
        e.preventDefault();
        let inputCount = $(this).prev().val();

        if (inputCount > 0) {
            inputCount++;
        } else {
            inputCount = 1;
        }

        $(this).prev().val(inputCount);

        let url = $(this).attr('href') + '/?count=' + inputCount;
        console.log('add');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.basketindexcontainer').html(data);
                fetch('/basket/getbasket')
                    .then(res => res.text())
                    .then(data => {
                        $('.header-cart').html(data);
                    });
            });
    })

    $(document).on('click', '.deletefromcartbtn', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.basketindexcontainer').html(data);
                fetch('/basket/getbasket')
                    .then(res => res.text())
                    .then(data => {
                        $('.header-cart').html(data);
                        fetch('/basket/DeleteUpdate')
                            .then(res => res.json())
                            .then(data => {
                                $('.notification').html(data);
                            });
                    });
            })
    })
    $(document).on('show.bs.collapse', '.accordian-body', function () {
        $(this).closest("table")
            .find(".collapse.in")
            .not(this)
    })

    //$(document).on('click', '#addtobasket', function (e) {
    //    $(this).preventDefault();
    //    let value = $('#myText').attr('value');
    //    fetch($(this).attr('href'))
    //        .then(res => res.text())
    //        .then(data => {
    //            $('.header-cart').html(data);
    //        })
    ////    $.ajax({
    ////        type: 'GET',
    ////        url: '@Url.Action("AddToBasket", "Basket")',
    ////        cache: false,
    ////        data: { myText: $("#myText").attr('value') },
    ////        success: function (data) {
    ////        },
    ////        error: function (req) {

    ////        }
    ////    });

    ////    // we make sure to cancel the default action of the link
    ////    // because we will be sending an AJAX call
    ////    return false;
    //});
    
})

