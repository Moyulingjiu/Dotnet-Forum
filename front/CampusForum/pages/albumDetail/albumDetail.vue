<template>
	<view>
		<button type="primary" @click="chooseAndUploadPic()">请选择图片</button>
		
		<view class="container":style="{'--picWidth':this.picWidth}">
			<view v-for="(item,index) in this.picture.Item">
				<template>
					<image :src="item.url" mode="aspectFill" class="text" @click="clickPic(index)"></image>
				</template>
			</view>
		</view>
		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="删除成功" :duration="3000"></uni-popup-message>
		</uni-popup>
	</view>
	
</template>


<script>
	import * as config from '../../utils/config.js'
	import * as pictureApi from '../../api/picture.js'
	export default {
		data() {
			return {
				delete:0,
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
		onNavigationBarButtonTap:function(e){
			if(this.delete==0){
				this.delete=1
				uni.showToast({
					title: '进入删除模式，点击相片删除，再次点击删除按钮退出删除模式',
					icon: "error",
					mask: true,
					duration: 2000
				})
			}
			else {
				this.delete=0
				uni.showToast({
					title: '退出删除模式',
					icon: "error",
					mask: true,
					duration: 2000
				})
			}
		    console.log(e.text);//提交
		
		    console.log(e.fontSize);//16px
			
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
			refresh() {
				if (config.checkToken()) {
					let len
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
							})
				} else {
					uni.redirectTo({
						url: '../login/login'
					})
				}
			},
			clickPic(index){
				let that=this
				console.log(this.delete)
				if(this.delete==0){
					window.location.href=String(this.picture.Item[index].url).replace(/\/api/g, "http://localhost:20673")
					uni.navigateTo({
									url: String(this.picture.Item[index].url).replace(/\/api/g, "http://localhost:20673")
								});
				}
				else
				{
					uni.showModal({
					    title: '确定要删除此相片吗?',
					    content: ' ',
					    success: function(res) {
					        if (res.confirm) {
					           console.log(that.picture)
								pictureApi.deletePicture(that.picture.Item[index].id).then(data=>{
									console.log(data)
									if (typeof data === "undefined") {
										uni.showToast({
											title: '服务器错误',
											icon: "error",
											mask: true,
											duration: 2000
										})
									} else if (data.code == 200) {
										that.$refs.popup_success.open('top')
										that.refresh();
									} else {
										uni.showToast({
											title: '删除失败:'+ data.msg,
											icon: "error",
											mask: true,
											duration: 2000
										})
									}
								})
					        }
					    }
					});
				}
			},
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

