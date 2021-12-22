<template>
	<view class="laugh">
		<!-- <button type="primary" @click="getImg">请选择图片</button> -->
		<uni-list>
			<!-- title、note设置标题和小文字 -->
			<uni-list-item direction="row" title="新增相册" @click="clickToAddAlbum()" link>
				<template v-slot:header>
					<image class="image-1" mode="aspectFill"></image>
				</template>
			</uni-list-item>
			<uni-list-item direction="row" v-for="(item,index) in albumArr" :key="index" :title="item.name"
				:note="item.description" @click="clickAlbum(item.id)" link>
				<!-- 通过v-slot:header插槽定义列表左侧的图片显示，其他内容通过List组件内置属性实现-->
				<template v-slot:header>
					<!-- 当前判断长度只为简单判断类型，实际业务中，根据逻辑直接渲染即可 -->
					<image class="image-1" :src="item.cover" mode="aspectFill"></image>
				</template>
			</uni-list-item>
		</uni-list>
		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="删除成功" :duration="3000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_success_refresh" type="message">
			<uni-popup-message type="success" message="刷新成功" :duration="2000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import uniList from "@/uni_modules/uni-list/components/uni-list/uni-list.vue" // uni-app列表组件，渲染图片列表
	import uniListItem from "@/uni_modules/uni-list/components/uni-list-item/uni-list-item.vue" // 列表项组件
	import * as config from "../../utils/config.js"
	import * as albumApi from "../../api/album.js"
	export default {
		data() {
			return {
				delete:0,
				albumArr: [{
						"id": "1",
						"name": "1",
						"description": "testaaaaaaaaaaaaaaaaaaaaaaaa",
						"cover": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
					},
				],
			}
		},
		onLoad() {
		},
		onNavigationBarButtonTap:function(e){
			if(this.delete==0){
				this.delete=1
				uni.showToast({
					title: '进入删除模式，点击相册删除，再次点击删除按钮退出删除模式',
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
		onShow() {
			this.refresh()
		},
		onPullDownRefresh() {
			this.refresh()
			uni.stopPullDownRefresh();
			this.$refs.popup_success_refresh.open('top')
		},
		methods: {
			refresh() {
				if (config.checkToken()) {
					albumApi.getAlbumByUserId().then(data => {
						this.albumArr = data.data.item;
					})
				} else {
					uni.redirectTo({
						url: '../login/login'
					})
				}
			},
			clickAlbum(albumId) {
				var that = this
				if(this.delete==0)
				{
					uni.navigateTo({
						url: `/pages/albumDetail/albumDetail?albumId=${albumId}`
					});
				}
				else
				{
					uni.showModal({
					    title: '确定要删除此相册吗?',
					    content: ' ',
					    success: function(res) {
					        if (res.confirm) {
					           
								albumApi.deleteAlbum(albumId).then(data=>{
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
											title: '删除失败'+ data.msg,
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
			clickToAddAlbum() {
				uni.navigateTo({
					url: '/pages/albumAdd/albumAdd',
				});
			},
			upload() {
				uni.uploadFile({
					url: '', //服务器地址
					fileType: "image", //ZFB必填,不然报错
					filePath: this.uploadImgArr[0], //这个就是我们上面拍照返回或者先中照片返回的数组
					name: 'imgFile',
					success: (uploadFileRes) => {
						let imgData = JSON.parse(uploadFileRes.data)
						console.log(UploadFileRes);
						console.log(this);
					}
				})
			},
			getImg() {
				uni.chooseImage({
					count: 6, // 最多可以选择的图片张数，默认9
					sizeType: ['original', 'compressed'], //original 原图，compressed 压缩图，默认二者都有
					sourceType: ['album', 'camera'], //album 从相册选图，camera 使用相机，默认二者都有。如需直接开相机或直接选相册，请只使用一个选项
					success: function(res) {
						console.log(JSON.stringify(res.tempFilePaths));
						this.uploadImgArr = res.tempFilePaths;
						console.log(this.uploadImgArr);
						this.upload();
					}
				});
			}

		}
	}
</script>

<style>
	.image-1 {
		flex-shrink: 0;
		margin-right: 10px;
		width: 25%;
		height: 125px;
		border-radius: 6px;
		overflow: hidden;
		border: 1px #f5f5f5 solid;
	}
</style>
