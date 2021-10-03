<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="thanh-toan.aspx.cs" Inherits="LAPTRINHWEB.thanh_toan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
    <title>Demo trả góp Alepay</title>
    <meta charset="UTF-8" name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0"/>
    <meta http-equiv="X-UA-Compatible" content="ie=edge"/>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>
     <link rel="stylesheet" href="Styles/Site.css"/>
</head>
<body>
    <div id="container" class="container">
        
        <div class="col-sm-14">
            <h2>Thông tin khách hàng</h2>         
            <form name="checkoutAlepay" class="form" id="formSubmit" method="post"  action="/thanh-toan.aspx?action=sendOrderToAlepay"> 
                <div class="form-group col-sm-5">
                    <label class="control-label">Tổng Tiền <span class="require">(*)</span></label>
                    <input runat="server" type="text" class="form-control" name="amount" id="amount" value="16000000"/>
                </div>
                <div class="form-group col-sm-5">     
                    <label class="control-label">Số Lượng <span class="require">(*)</span></label>
                    <input runat="server" type="text" placeholder="Số Lượng" class="form-control" name="totalItem" id="totalItem" value="1"/>
                </div>
                 <div class="form-group col-sm-5">     
                    <label class="control-label">Email <span class="require">(*)</span></label>
                    <input runat="server" type="text" value="hung@gmail.com" placeholder="Email" class="form-control" name="buyerEmail" id="buyerEmail" />
                </div>
                <!-- Text input-->
                <div class="form-group col-sm-5">
                    <label class="control-label">Tiền Tệ <span class="require">(*)</span></label>
                    <select runat="server" name="currency" id="currency" class="form-control">
                        <option value="VND">VND</option>
                        <option value="USD">USD</option>
                    </select>
                </div>
                <!-- Text input-->
                <div class="form-group col-sm-5">
                    <label class="control-label">Họ Tên <span  class="require">(*)</span></label>
                    <input runat="server" type="text" value="Hùng" placeholder="Tên" class="form-control" name="buyerName" id="buyerName" />
                </div>
                <div class="form-group col-sm-5">     
                    <label class="control-label">Số Điện Thoại <span class="require">(*)</span></label>
                    <input runat="server" type="text" value="0848506079" placeholder="Số Điện Thoại" class="form-control" name="buyerPhone" id="buyerPhone"/>
                </div>
                <!-- Text input-->
                <div class="form-group col-sm-5">
                    <label class="control-label">Địa Chỉ <span class="require">(*)</span></label>
                    <input runat="server" type="text" value="209 Quốc lộ 13" placeholder="Địa Chỉ" class="form-control" name="buyerAddress" id="buyerAddress"/>
                </div>
                <div class="form-group col-sm-5">     
                    <label class="control-label">Quốc gia<span class="require">(*)</span></label>
                    <input runat="server" type="text" value="Việt Nam" placeholder="" class="form-control" name="buyerCountry" id="buyerCountry" />
                </div>
                  <div class="form-group col-sm-5">     
                    <label class="control-label">Thành Phố <span class="require">(*)</span></label> 
                    <input runat="server" type="text" placeholder="Thành Phố" class="form-control" name="buyerCity" id="buyerCity"  value="Thành phố Hồ Chí Minh"/>
                </div>
                <!-- Text input-->
                <div class="form-group col-sm-5">
                    <label class="control-label" for="orderDescription">Mô Tả Hóa Đơn<span class="require">(*)</span></label>
                    <textarea rows="2" cols="100" runat="server" placeholder="Thông Tin Mô Tả Hóa Đơn" id="orderDescription" name="orderDescription" class="form-control" required="">Mua thịt cá trứng sữa</textarea>
                </div>
                <div class="row"></div>
                <div class="col-sm-10" id="alert">
                  <!--   <div runat="server" id="showresultErrors" class="alert alert-danger"></div>-->
                </div>
                <div class="form-group col-sm-5">
                    <p>&nbsp;</p>                    
                     <button id="sendInstallment" type="submit" class="btn btn-info btn-lg">
                             Thanh toán 
                     </button>
                </div>            
            </form>
        </div>
    </div>
    <div id="sendOrderToAlepayInstallment" class="modal fade in" role="dialog" runat="server">
            <iframe id="frame" scrolling="no" style="overflow: hidden;height: 100%;width: 100%;border: none;"></iframe>
    </div><!-- /.row -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        $('#sendInstallment').on('click', function () {
            $('#alert').html('Vui lòng chờ ít phút...');
            $.ajax({
                type: "POST",
                url: $("#formSubmit").prop('action'),
                data: $("#formSubmit").serialize(), // serializes the form's elements.
                success: function (data) {
                    console.log(data.errorCode);
                    if (typeof data.errorCode != 'undefined' || data.checkoutUrl=='') {
                        $('#alert').html('<div class="alert alert-danger">' + data.errorDescription + '</div>');
                        return false;
                    } else {
                        $('#frame').prop('src', data.checkoutUrl);
                        $('#sendOrderToAlepayInstallment').modal('show');
                        $('#alert').html('');
                    }
                }
            });
            return false;
        });
        $('#frame').on('load', function () {
            this.style.height = this.contentDocument.body.scrollHeight + 'px';
        });
    </script>
</body>
</html>