$(document).ready(function () {
    //delete 
    $(".delete").click(function () {
        console.log({ id: $(this).data("id") });
        let isDelete = confirm("Are you sure you want to delete?");
        let id = $(this).data("id");
        if (isDelete) {
            $.ajax({
                url: "DeleteJobInfo",
                type: "POST",
                data: { id: $(this).data("id") }

            })/*.done(function (response) {
               
                  
                    $('#jobInfo').load("/App/ShowJobInfoUser #jobInfo");
        
                    //refreshJobInfoList();
                
                
            })*/
        }
    })
    /*function refreshJobInfoList() {
        $.ajax({
            url: "/App/ShowJobInfoByUser", // Action lấy danh sách công việc
            type: "GET",
            success: function (data) {
                // Duyệt qua danh sách công việc và tạo HTML để hiển thị
                let html = "";
                $.each(data, function (index, jobInfo) {
                    html += "<div>";
                    // Thêm các thông tin công việc vào html
                    html += "</div>";
                });
                // Cập nhật nội dung của .container bằng danh sách công việc mới
                $(".container").html(html);
            },
            error: function () {
                alert("Có lỗi xảy ra khi lấy danh sách công việc.");
            }
        });
    }*/
    function refreshJobInfoList() {
        $.ajax({
            url: "/App/ShowJobInfoUser", // Action lấy danh sách công việc
            type: "GET",
            success: function (data) {
                //$("#jobInfo").html(data); // Cập nhật nội dung của phần tử chứa danh sách công việc
                $('#jobInfo').load("/App/ShowJobInfoUser #jobInfo"); // Cập nhật lại nội dung bảng tbody
            },
            error: function () {
                alert("Có lỗi xảy ra khi lấy danh sách công việc.");
            }
        });
    }
})
$(document).ready(function () {
    // Gắn sự kiện submit cho form
    $('#addJobForm').submit(function (event) {
        event.preventDefault(); // Ngăn chặn gửi form mặc định

        // Lấy dữ liệu từ form
        var formData = $(this).serialize();

        // Thực hiện yêu cầu Ajax
        $.ajax({
            url: '@Url.Action("AddJobs", "App")', // URL của action để xử lý yêu cầu Ajax
            type: 'POST',
            data: formData, // Dữ liệu của form được gửi đi
            success: function (result) {
                // Xử lý kết quả trả về (nếu cần)
                console.log(result);
                // Cập nhật lại nội dung trang nếu cần
                // Ví dụ: window.location.href = '@Url.Action("Index", "App")';
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });
});
