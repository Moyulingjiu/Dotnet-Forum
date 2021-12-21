<template>
	<view>
		<view class="tabs">
		    <scroll-view scroll-x class="scroll-h" >
		    <block v-for="(tab,index) in tabBars" :key="tab.id">
		    <view class="uni-tab-item" :class="{'uni-tab-item-title-active' :tabIndex==index}" @tap="tabtap(index)">
		    <!-- 1 {{tab.name}} -->
				<text class="uni-tab-item-title" :class="tabIndex==index ? 'uni-tab-item-title-active' : ''">{{tab.name}}</text>
		    </view>
		    </block>            
		    </scroll-view>
		</view>
		<view v-if="tabIndex==0">
			<table :style="{'width':this.windowWidth}" border="1px solid #ccc" cellspacing="0" cellpadding="0">
				<thead>
					<tr>
						<th class="idRow">id</th>
						<th class="usernameRow">username</th>
						<th>通 过</th>
						<th>取 消</th>
					</tr>
				</thead>
				<br>
				<tr v-for="(item,index) in applicationList.data">
					<th class="idRow">{{item.id}}</th>
					<th class="usernameRow">{{item.username}}</th>
					<th><button class="passButton" @click="cilckPass(item.id)">通 过</button></th>
					<th><button class="cancelButton" @click="cilckCancel(item.id)">取 消</button></th>
				</tr>
			</table>
		</view>
		
		<view v-if="tabIndex==1">
			<table :style="{'width':this.windowWidth}" border="1px solid #ccc" cellspacing="0" cellpadding="0">
				<thead>
					<tr>
						<th class="idRow">id</th>
						<th class="usernameRow">username</th>
						<th>封 禁</th>
						<th>注 销</th>
					</tr>
				</thead>
				<br>
				<tr v-for="(item,index) in userList.data">
					<th class="idRow">{{item.id}}</th>
					<th class="usernameRow">{{item.username}}</th>
					<th><button class="passButton" @click="cilckPass(item.id)">封 禁</button></th>
					<th><button class="cancelButton" @click="cilckCancel(item.id)">注 销</button></th>
				</tr>
			</table>
		</view>
		
		<uni-pagination id="pagination"
		    show-icon="true" 
		    :total="applicationList.totle" 
		    :current="applicationList.page"
			:pageSize="applicationList.pageSize"
			@change="pageChange">
		</uni-pagination>
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
						},
						{
							"id":"2",
							"username":"s",
						},
						{
							"id":"3",
							"username":"b",
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
							"username":"xxxx"
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
</style>
