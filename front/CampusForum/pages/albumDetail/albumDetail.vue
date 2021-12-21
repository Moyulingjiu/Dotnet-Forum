<template>
	<view>
		<button type="primary" @click="getImg">请选择图片</button>
		<image :src="picPath"></image>
		<view class="container":style="{'--picWidth':this.picWidth}">
			<view v-for="(item,index) in pictrue.Item">
				<template>
					<image :src="item.url" mode="aspectFill" class="text" @click="clickPic(item.Id)"></image>
				</template>
			</view>
		</view>
	</view>
</template>


<script>
	export default {
		data() {
			return {
				picPath:"",
				windowWidth:0,
				picWidthNum:0,
				picWidth:"",
				picRowNum:0,
				pictrue:{
					Totle:3,
					Item:[
						{
							"Id":"1",
							"Name":"asd",
							"url":"https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						},
						{
							"Id":"2",
							"Name":"asdsad",
							"url":"https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						},
						{
							"Id":"3",
							"Name":"asdddd",
							"url":"https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						}
					]
				}
			}
		},
		onLoad() {
			//获取picture数据，还要传page和pagesize
			
			console.log(123);
			var that=this;
			uni.getSystemInfo({
			    success: function (res) {
			        console.log(res.windowWidth);
			        console.log(res.windowHeight);
					that.windowWidth=res.windowWidth;	
					that.picRowNum=Math.ceil(Math.max(3,that.windowWidth/250));
					console.log(that.picRowNum);
					that.picWidthNum=(that.windowWidth-1)/that.picRowNum;
					that.picWidth=that.picWidthNum+"px";
			    }
			});
		},
		
		methods: {
			clickPic(pictrueId){
				uni.navigateTo({
				                url: '/pages/albumDetail/albumDetail?pictrueId='+{pictrueId}.pictrueId,
				            });
			},
			upload(){
				console.log(123);
				var that = this
				uni.uploadFile({
					url: '/api/insert/1', //服务器地址
					fileType:"image",//ZFB必填,不然报错
					filePath: that.uploadImgArr[0],//这个就是我们上面拍照返回或者先中照片返回的数组
					name: 'imgFile',
					success: (uploadFileRes) => {
						let imgData = JSON.parse(uploadFileRes.data)
						console.log(UploadFileRes);
						console.log(this);
					}
				});
			},
			getImg () {
				var that = this
				uni.chooseImage({
				    count: 6,           // 最多可以选择的图片张数，默认9
				    sizeType: ['original', 'compressed'],              //original 原图，compressed 压缩图，默认二者都有
				    sourceType: ['album', 'camera'],             //album 从相册选图，camera 使用相机，默认二者都有。如需直接开相机或直接选相册，请只使用一个选项
				    success: function (res) {
				        console.log(JSON.stringify(res.tempFilePaths));
						that.picPath=JSON.stringify(res.tempFilePaths)+'.png';
						that.uploadImgArr = res.tempFilePaths;
						console.log(that.uploadImgArr);
						//that.upload();
				    }
				});
			},
		}
	}
</script>

<style>
	.container{
		/* 定义flex容器 */
		display: flex;
         /*设置容器内部容器的排列方向*/	
		flex-direction: row; 
		flex-wrap: wrap;
	}
	.green{
		background-color: green;
	}
	.blue{
		background-color: blue;
	}
	.red{
		background-color: red;
	}
	.black{
		background-color: black;
	}
	.orange{
		background-color: orange;
	}
	.text{
		font-size: 20upx;
		width: var(--picWidth);
		height: var(--picWidth);
	}
</style>

