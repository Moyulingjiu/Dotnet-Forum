<template>
	<view>
		<button type="primary" @click="chooseAndUploadPic()">请选择图片</button>
		
		<view class="container":style="{'--picWidth':this.picWidth}">
			<view v-for="(item,index) in this.picture.Item">
				<template>
					<image :src="item.url" mode="aspectFill" class="text" @click="clickPic(item.Id)"></image>
				</template>
			</view>
		</view>
	</view>
</template>


<script>
	import * as config from '../../utils/config.js'
	import * as pictureApi from '../../api/picture.js'
	export default {
		data() {
			return {
				albumId:0,
				windowWidth:0,
				picWidthNum:0,
				picWidth:"",
				picRowNum:0,
				pageInfo:{page:0,pageSize:20},
				picture:{
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
		onLoad(prop) {
			//获取picture数据，还要传page和pagesize
			let len
			this.albumId=prop.albumId;
			
			this.picture=pictureApi.getPictureByAlbumId(this.albumId,this.pageInfo).then(data => {
						console.log("ok");
						len=data.data.item.length
						console.log(data);
						this.picture={totle:0,Item:[]};
						console.log("len:"+len);
						for(let i=0;i<len;i++)
						{
							console.log("i:"+i);
							let temp={"id":data.data.item[i].id,
										"name":data.data.item[i].name,
										"url":data.data.item[i].url}
							this.picture.Item.push(temp);
							console.log(this.picture.Item)
							this.picture.Item[i].url="/api"+String(this.picture.Item[i].url).replace(/\\/g, "/");
						}
						console.log(this.picture.Item);
					});
			
			
			
			console.log(123);
			var that=this;
			uni.getSystemInfo({
			    success: function (res) {
			        //console.log(res.windowWidth);
			        //console.log(res.windowHeight);
					that.windowWidth=res.windowWidth;	
					that.picRowNum=Math.ceil(Math.max(3,that.windowWidth/250));
					//console.log(that.picRowNum);
					that.picWidthNum=(that.windowWidth-1)/that.picRowNum;
					that.picWidth=that.picWidthNum+"px";
			    }
			});
		},
		
		methods: {
			// clickPic(pictureId){
			// 	uni.navigateTo({
			// 	                url: '/pages/albumDetail/albumDetail?pictureId='+{pictureId}.pictureId,
			// 	            });
			// },
			chooseAndUploadPic(){
				var that=this;
			    uni.chooseImage({
			        count: 1,
			        sizeType:['copressed'],
			        success(res) {
			            //因为有一张图片， 输出下标[0]， 直接输出地址
			            var imgFiles = res.tempFilePaths[0]
						
			            console.log(imgFiles)
						console.log(config.getToken())
			            // 上传图片
			            // 做成一个上传对象
			            var uper = uni.uploadFile({
			                // 需要上传的地址
			                url:'/api/picture/insert/'+that.albumId,
							header: {
								'token': config.getToken(),
							},
			                // filePath  需要上传的文件
			                filePath: imgFiles,
			                name: 'file',
			                success(res1) {
			                    // 显示上传信息
			                    console.log(res1)
								uni.navigateTo({
				                url: '/pages/albumDetail/albumDetail?albumId='+String(that.albumId),
								});
			                }
			            });
			        }
			    })
				
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

