<template>
	<view class="big">
		<view class="trade">
			<view class="texts" :class="curr==0?'active':''" data-index="0" @tap="setCurr">
				关注
			</view>
			<view class="texts" :class="curr==1?'active':''" data-index="1" @tap="setCurr">
				粉丝
			</view>
		</view>
		<swiper :current="curr" @change="setCurr">
			<swiper-item>
				<scroll-view>
					<view v-for="(item,index) in applicationList.data">
						<view class="person_container">
							<view class="user">
								<text class="user_name">{{ item.username }}</text>
								<!-- <br />
								<text class="user_description">{{ item.description }}</text> -->
							</view>
							<view :class="item.isPassed?'follow':'unfollow'">
								{{ item.isPassed?'通过':'已通过' }}
							</view>
						</view>
					</view>
				</scroll-view>
			</swiper-item>
			<swiper-item>
				<scroll-view>
					<view v-for="(item,index) in userList.data">
						<view class="person_container">
							<image class="user_avater" :src="item.avater"></image>
							<view class="user">
								<text class="user_name">{{ item.username }}</text>
								<br />
								<text class="user_description">{{ item.description }}</text>
							</view>
							<view :class="item.isBanned?'follow':'unfollow'">
								{{ item.isBanned?'封禁':'已封禁' }}
							</view>
						</view>
					</view>
				</scroll-view>
			</swiper-item>
		</swiper>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				windowWidth:'',
				applicationList:{
					"totle":"50",
					"page":"1",
					"pageSize":"10",
					"data":[
						{
							"id":"1",
							"username":"facedawn",
							"isPassed":0
						},
						{
							"id":"2",
							"username":"s",
							"isPassed":1
						},
						{
							"id":"3",
							"username":"b",
							"isPassed":0
						},
					]
				},
				userList:{
					"totle":"50",
					"page":"1",
					"pageSize":"10",
					"data":[
						{
							"id":"21313",
							"username":"xxxx",
							"avater":"",
							"description":"",
							"isbanned":0
						}
					]
				},
				 tabIndex:0,
				    tabBars:[
				        { name:"审核注册",id:"application"},
				        { name:"查看用户",id:"user"},
				    ]
			}
		},
		onLoad() {
			//获取applicationList数据，还要传page和pagesize
			
			console.log(123);
			var that=this;
			uni.getSystemInfo({
			    success: function (res) {
					that.windowWidth=res.windowWidth+'px';	
			    }
			});
		},
		methods: {
			cilckPass(id){
				console.log(id+"pass");
			},
			cilckCancel(id){
				console.log(id+"cancel");
			},
			pageChange(e){
				//重新加载applicationList
				this.applicationList.page=e.current;
				console.log(this.applicationList.page);
			},
			 tabtap(index){
			        this.tabIndex=index;
			    }
		}
	}
</script>

<style>
	.idRow{
		width: 10%;
	}
	.usernameRow{
		width: 49%;
	}
	.passButton{
		background-color: #18BC37;
	}
	.cancelButton{
		background-color: #E43D33;
	}
	.tabs {
	    flex: 1;
	    flex-direction: column;
	    overflow: hidden;
	    background-color: #ffffff;
	    /* #ifdef MP-ALIPAY || MP-BAIDU */
	    height: 100vh;
	    /* #endif */
	}
	
	.scroll-h {
	    width: 750upx;
	    height: 80upx;
	    flex-direction: row;
	    /* #ifndef APP-PLUS */
	    white-space: nowrap;
	    /* #endif */
	    /* flex-wrap: nowrap; */
	    /* border-color: #cccccc;
	    border-bottom-style: solid;
	    border-bottom-width: 1px; */
	}
	
	.line-h {
	    height: 1upx;
	    background-color: #cccccc;
	}
	
	.uni-tab-item {
	    /* #ifndef APP-PLUS */
	    display: inline-block;
	    /* #endif */
	    flex-wrap: nowrap;
	    padding-left: 34upx;
	    padding-right: 34upx;
	}
	
	.uni-tab-item-title {
	    color: #555;
	    font-size: 30upx;
	    height: 80upx;
	    line-height: 80upx;
	    flex-wrap: nowrap;
	    /* #ifndef APP-PLUS */
	    white-space: nowrap;
	    /* #endif */
	}
	
	.uni-tab-item-title-active {
	    color: #007AFF;
	}
	
	.trade {
		margin-top: 30rpx;
		width: 100%;
		overflow: auto;
	}
	
	.trade view {
		text-align: center;
		width: 50%;
		float: left;
		font-size: 40rpx;
		font-weight: bold;
		padding-bottom: 10rpx;
	}
	
	.trade .texts.active {
		border-bottom: 8rpx solid #00A1D6;
	}
	
	.person_container {
		margin-bottom: 20rpx;
		padding: 10rpx;
		display: flex;
	}
	
	.user_avater {
		width: 100rpx;
		height: 100rpx;
		border-radius: 50%;
	}
	
	.user {
		margin-left: 15rpx;
	}
	
	.user_name {
		font-size: 45rpx;
		font-weight: bold;
	}
	
	.user_description {
		font-size: 35rpx;
		color: #555555;
	}
	
	.unfollow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #83cbac;
	}
	
	.follow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #b5aa90;
		/* box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); */
	}
</style>
