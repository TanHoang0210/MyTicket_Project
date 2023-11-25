<template>
    <div>
        <div  style="background-color: rgb(226, 168, 140);
         width: 50%;flex-direction: column; margin: auto;
         ">
          <h1 style="font-weight: bold; width: 100%;
          text-align: center;
           background-color:rgb(188, 101, 60) ; 
           color: white;
           padding: 10px 0;
           ">
           MyTicket - Ứng dụng đặt vé số 1 Việt Nam
        </h1>
         <div style=" display: flex; padding: 20px 0;">
             <div>
                 <img style="width: 200px; height: 200px;" src="https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png" alt="">
             </div>
             <div style="margin: auto; flex-direction: column; text-align: center; color: #555;">
     
 
                  <h2>ĐƠN HÀNG #{order.OrderCode} đã được đặt 
                     <span style="color: green;">
                         thành công!
                     </span>
                 </h2>
                  <button style="background-color: rgb(188, 101, 60);height: 50px; margin: auto;
                   font-size: 20px; border-radius: 4px 4px; ">
                      <a style="text-decoration: none; color: white;"  href="http://localhost:8080/order">Kiểm tra vé của bạn tại đây</a>
                  </button>
             </div>
         </div>
        </div>
    </div>
</template>

<script>

</script>

<style>

</style>