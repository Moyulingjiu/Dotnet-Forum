<template>
    <view>
        <progress :percent="percent" strock-width="10"></progress>
        <button type="primary" @tap="cI">chooseImg</button>
    </view>
</template>

<script>
    // 注册一个进度条
    var _self;

    export default {
        data() {
            return {
                percent:0
            }
        },
        onLoad() {
            _self = this;
        },
        methods: {
            // cI(){
            //     uni.chooseImage({
            //         count: 1,
            //         sizetype: ['compressed'],
            //         success(res){
            //             // tepFliePaths 保存图片路径
            //             var imgFiles = res.tempFilePaths;
            //             // 因为没写下标， 直接以数组形式输出
            //             console.log(imgFiles)
            //         }
            //     })
            // }
            cI:function(){
                uni.chooseImage({
                    count: 1,
                    sizeType:['copressed'],
                    success(res) {
                        //因为有一张图片， 输出下标[0]， 直接输出地址
                        var imgFiles = res.tempFilePaths[0]
                        console.log(imgFiles)
                        // 上传图片
                        // 做成一个上传对象
                        var uper = uni.uploadFile({
                            // 需要上传的地址
                            url:'http://localhost:44330/insert/1',
                            // filePath  需要上传的文件
                            filePath: imgFiles,
                            name: 'file',
                            success(res1) {
                                // 显示上传信息
                                console.log(res1)
                            }
                        });
                        // onProgressUpdate 上传对象更新的方法
                        uper.onProgressUpdate(function(res){
                            // 进度条等于 上传到的进度
                            _self.percent = res.progress
                            console.log('上传进度' + res.progress)
                            console.log('已经上传的数据长度' + res.totalBytesSent)
                            console.log('预期需要上传的数据总长度' + res.totalBytesExpectedToSend)
                        })
                    }
                })
            }
        }
    }
</script>

<style>

</style>