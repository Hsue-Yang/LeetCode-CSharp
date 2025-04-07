using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using static Program;

internal class Program
{
    private readonly string _programName;
    public int MyProperty { get; set; }
    private static void Main(string[] args)
    {
        #region twoDimensional
        //int[,] twoDimensional = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
        //int sum = 0;
        //for (int i = 0; i < twoDimensional.GetLength(0); i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        sum += twoDimensional[i, j];
        //    }
        //    Console.WriteLine(sum);
        //    sum = 0;
        //}
        //int number = Convert.ToInt32(Console.ReadLine());
        //if (Myfunction(number))
        //{
        //    Console.WriteLine("True");
        //}
        //else
        //{
        //    Console.WriteLine("False");
        //}
        #endregion
        #region Add Two Numbers
        //var l1 = new ListNode(2, new ListNode(4, new ListNode(3)));
        //var l2 = new ListNode(5, new ListNode(6, new ListNode(4)));
        //var a = AddTwoNumbers(l1, l2);

        //while (a.next != null || a.val != 0)
        //{
        //    Console.WriteLine(a.val);
        //    a = a.next;
        //}
        ////var head = new ListNode(1, new ListNode(2, new ListNode(3, new ListNode(4, new ListNode(5)))));
        //var list1 = new ListNode(1, new ListNode(2, new ListNode(4)));
        //var list2 = new ListNode(1, new ListNode(3, new ListNode(4)));
        #endregion

        Console.WriteLine(MergeTwoLists(list1, list2));
        Console.ReadKey();
    }

    #region 1. Two Sum
    public int[] TwoSum(int[] nums, int target)
    {
        Dictionary<int, int> num = new Dictionary<int, int>();

        for (int i = 0; i < nums.Length; i++)
        {
            int diff = target - nums[i];
            if (num.ContainsKey(diff))
            {
                return new int[] { num[diff], i };
            }

            if (!num.ContainsKey(nums[i]))
            {
                num.Add(nums[i], i);
            }
        }
        return new int[2];
    }
    #endregion

    #region 2. Add Two Numbers (ListNode)
    //public class ListNode
    //{
    //    public int val; //當下節點數值
    //    public ListNode next; //指向下一個節點
    //    public ListNode(int value = 0, ListNode nextNode = null)
    //    {
    //        val = value;
    //        next = nextNode;
    //    }
    //}
    public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
    {
        ListNode result = new ListNode(); //用來儲存節點值，不然next會被蓋過
        ListNode current = result;
        int carry = 0;
        //節點與節點相加，當超過個位數進位到下一節點
        while (l1 != null || l2 != null || carry != 0)
        {
            int sum = (l1?.val ?? 0) + (l2?.val ?? 0) + carry;
            carry = sum / 10; //進位
            current.next = new ListNode(sum % 10); //定義相加後的數值
            current = current.next;

            l1 = l1?.next;
            l2 = l2?.next;
        }

        return result.next;
    }
    #endregion

    #region 3. Longest Substring Without Repeating Characters
    public int LengthOfLongestSubstring(string s)
    {
        int[] charIndexes = new int[128]; //ASCII編碼128字元  
        //'A' 的 ASCII 值是 65，所以 (int)'A' 等於 65。
        int longestSubstring = 0;
        int startIndex = 0;

        for (int i = 0; i < s.Length; i++)
        {
            startIndex = Math.Max(startIndex, charIndexes[(int)s[i]]); //當遇到重複直接將Index設置到那

            longestSubstring = Math.Max(longestSubstring, i - startIndex + 1); //計算最長不重複的substring

            charIndexes[(int)s[i]] = i + 1; //在ASCII陣列設置陣列索引
        }

        return longestSubstring;

        //HashSet<char> set = new HashSet<char>();
        //int left = 0, maxLength = 0;
        //for (int right = 0;right < s.Length; right++)
        //{
        //    while (set.Contains(s[right]))//移除到沒有同樣值為主，避免重複
        //    {
        //        set.Remove(s[left]);
        //        left++;
        //    }
        //    set.Add(s[right]);
        //    maxLength = Math.Max(maxLength, right - left + 1);//長度left到right是+1個字符
        //}

        //return maxLength;
    }
    #endregion

    #region 4. Median of Two Sorted Arrays (中位數)
    public double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        //二分法
        //定義兩個陣列的分割線在 mid=  (nums1.Length + nums2.Length + 1) / 2 (考慮奇偶數一致、一定會是整數、奇數右側會多一個)
        //nums1[mid - 1]< nums2[mid] && nums1[mid] > nums2[mid - 1]
        //分割線左邊的值一定小於右邊的值
        //記得考慮極端狀況，分割線左右可能為空
        //中位數 => 當陣列長度和為奇數 答案為 Math.Max(nums1[mid - 1], nums2[mid -1])
        //中位數 => 當陣列長度和為偶數 答案為 (Math.Max(nums1[mid - 1], nums2[mid -1]) + Math.Min(nums1[mid], nums2[mid])) / 2

        //對陣列長度較長的陣列找分割線
        //假設nums1.Length > nums2.Length
        //int left = 0, right = nums1.Length
        //從left 找到 right，定義left ~ right的分割線為 int i = left + (right - left + 1)/2 ，讓查找範圍越縮越小
        //這時在nums2的分割線定義為 int j = mid - i
        //當不滿足nums1[i - 1]< nums2[j]條件，代表要在往內縮 nums1的值太大，right = i-1
        //當滿足時，表示分割線的i夠小，可以繼續往右找 left = i
        if (nums1.Length > nums2.Length)
        {
            int[] temp = nums1;
            nums1 = nums2;
            nums2 = temp;
        }
        int m = nums1.Length, n = nums2.Length;
        int mid = (n + m + 1) / 2;
        int left = 0, right = m;
        while (left < right)
        {
            int ii = left + (right - left + 1) / 2; //for nums1
            int jj = mid - ii; //for nums2
            if (nums1[ii - 1] > nums2[jj])
            {
                right = ii - 1;//nums1[i - 1]值過大
            }
            else
            {
                left = ii;
            }
        }
        int i = left;
        int j = mid - i;
        int nums1LeftMax = i == 0 ? int.MinValue : nums1[i - 1];
        int nums1RightMin = i == m ? int.MaxValue : nums1[i];
        int nums2LeftMax = j == 0 ? int.MinValue : nums2[j - 1];
        int nums2RightMin = j == n ? int.MaxValue : nums2[j];
        if ((n + m) % 2 == 1) //奇數
        {
            return Math.Max(nums1LeftMax, nums2LeftMax); //因為nums1的陣列長度較長，取大值
        }
        else
        {
            return (double)(Math.Max(nums1LeftMax, nums2LeftMax) + Math.Min(nums1RightMin, nums2RightMin)) / 2;
        }
    }
    #endregion

    #region 5. Longest Palindromic Substring
    public string LongestPalindrome(string s)
    {
        //目的:根據上面i++一直去擴展，等於一段字一段字找
        //例如:"babad"，"ba", "bab", "baba","babad"這樣找
        //使用pointer，while 配上left right
        //從0,1開始找，如果有就更新substring
        //沒配對到就right++，有就會left++找
        int start = 0, maxLength = 1;
        for (int i = 0; i < s.Length; i++)
        {
            //i 要視為中心
            //奇數
            int left = i, right = i;
            while (left >= 0 && right < s.Length && s[left] == s[right])
            {
                left--;
                right++;
            }
            int len1 = right - left - 1; //-1是多一次向外擴展
            //偶數
            int left2 = i, right2 = i + 1;
            while (left >= 0 && right < s.Length && s[left] == s[right])
            {
                left--;
                right++;
            }
            int len2 = right2 - left2 - 1;
            int len = Math.Max(len1, len2);
            if (len > maxLength)
            {
                maxLength = len;
                start = i - (len - 1) / 2;//-1是為了符合奇數跟偶數的條件
                //(len - 1) / 2 就是起點到中心的距離 (半徑)
            }
        }
        return s.Substring(start, maxLength);
    }

    private int ExpandAroundCenter(string s, int left, int right)
    {
        //中心擴展法
        //從0++找到n++，如果兩個都符合就left--跟right++
        //目的:根據上面i++一直去擴展，等於一段字一段字找
        //例如:"babad"，"ba", "bab", "baba","babad"這樣找
        while (left >= 0 && right < s.Length && s[left] == s[right])
        {
            left--; //一直找到left變0跳出迴圈
            right++;//一直找到right超出s.Length跳出迴圈
                    //s[left] != s[right]也會跳出迴圈
        }
        return right - left - 1; //-1是因為在不滿足條件時left-1跟right+1還會有一次多餘的個數
    }
    #endregion

    #region 6. Zigzag Conversion
    // 字母按照Z字形的排法由上到下，在由下到上，依照給定行數
    // 例如: "ABCDEFGHIJKLMNO",4row 
    // A    G    M
    // B  F H  L N
    // C E  I K  O
    // D    J
    //總共四行，zigzag就是z字形排法
    //也可以用周期方式去解。例如從A->D 是numsRow-1步
    //從D->G之前 是numsRow-2步，從A->F是 (numsRow-1)+(numsRow-2) = 2*numsRow -2
    //利用這個周期公式去一個一個抽出來組成新字串，每隔周期就取字符
    public string ZigzagConvert(string s, int numRows)
    {
        if (numRows == 1 || s.Length <= numRows)
        {
            return s;
        }
        //利用List的特性有numRows列，分別插入字母再把它合併起來
        //判斷現在的列數，使用是否往下去+1或-1
        List<string> rows = new List<string>();
        for (int i = 0; i < numRows; i++)
        {
            rows.Add("");
        }
        int currentRow = 0;
        bool goingDown = false;
        foreach (var c in s)
        {
            rows[currentRow] += c;

            if (currentRow == 0 || currentRow == numRows - 1)
            {
                goingDown = !goingDown;
            }
            currentRow = goingDown ? currentRow + 1 : currentRow - 1;
        }
        string result = string.Join("", rows);

        return result;
    }
    #endregion

    #region 7. Reverse Integer 
    //Out of Range[-2^31, 2^31 - 1] return 0
    //[-2147483648, 2147483647]32bit範圍
    //Only accept 32-bit integer non 64-bit
    public int Reverse(int x)
    {
        int result = 0;
        while (x != 0)
        {
            int digit = x % 10;
            if (result > int.MaxValue / 10 || result == int.MaxValue / 10 && digit > 7)//因為result*10+digit不能超過2147483647
            {
                return 0;
            }
            if (result < int.MinValue / 10 || result == int.MinValue / 10 && digit < -8)//因為result*10+digit不能超過-2147483648
            {
                return 0;
            }
            result = result * 10 + digit;
            x /= 10;
        }

        return result;
    }
    #endregion

    #region 8. String to Integer(atoi) 
    //目的: 將string根據以下條件轉換成int
    //重點: 了解char是用ASCII去編碼，利用 char - '0'的方式。
    //例如: char'1' = 49, char'0' = 48，要得到數字1就是用 char - char'0' = 數字
    //1. 去除leading空白
    //2. 確認有沒有"-"或"+"，預設為"+"
    //3. 去除開頭0，如果都不是數字就返回0
    //4. 確認範圍是否在32bit [-2^31, 2^31]內
    //5. 遇到非數字就不用繼續往後查了
    public int MyAtoi(string s)
    {
        string init = s.Trim();
        int result = 0;
        int index = 0;
        bool IsNegative = false;
        if (init.Length > 0 && init[0] == '-')
        {
            IsNegative = true;
            index++;
        }
        if (init.Length > 0 && init[0] == '+')
        {
            index++;
        }
        for (int i = index; i < init.Length; i++)
        {
            if (init[i] < '0' || init[i] > '9')// 判斷是不是數字
            {
                break;
            }
            int num = init[i] - '0';
            if (result > (int.MaxValue - num) / 10)
            {
                return IsNegative ? int.MinValue : int.MaxValue;
            }
            result = result * 10 + num;
        }

        return IsNegative ? -result : result;
    }
    #endregion

    #region 9. Palindrome Number 
    //題目有限定:不轉換成string解決
    public bool IsPalindrome(int x)
    {
        if (x < 0) return false; //負數不是回文
        if (x >= 0 && x < 10) return true; //一個數字算回文
        int result = 0, ori = x;
        while (ori != 0)
        {
            int num = ori % 10;
            ori /= 10;
            if (result > (int.MaxValue - num) / 10) return false;
            result = result * 10 + num;
        }
        return x == result;
    }
    #endregion

    #region 10. Regular Expression Matching
    //"."可以匹配任何單個字符
    //"*"匹配零個會多個前一元素
    //s及p範圍在1~20之間, s跟p只會有小寫, p的特殊符號只會有"."跟"*"
    //Dynamic Programming解法，二維動態規劃表dp

    public bool IsMatch(string s, string p)
    {
        return IsMatch(s, p, 0, 0, new bool[s.Length + 1, p.Length]); //索引從0開始所以要預留1個位置
    }
    private bool IsMatch(string s, string p, int sIdx, int pIdx, bool[,] dp)
    {
        if (sIdx == s.Length && pIdx == p.Length) return true; //index
        if (pIdx == p.Length) return false; //pattern結束但是字串還沒結束 匹配失敗

        if (dp[sIdx, pIdx]) return false; //儲存已匹配過，如果為true，代表下面那行已經計算過
        dp[sIdx, pIdx] = true; //儲存代表已處理過
        var ch = sIdx == s.Length ? ' ' : s[sIdx]; //匹配到string的尾端，最後一個用空字串代替(因為p還沒到尾端)
        var th = p[pIdx];
        if (pIdx + 1 < p.Length && p[pIdx + 1] == '*')
        {
            //假設不使用星號匹配 跳過星號去匹配 pIdx +2 (因為上面+1這邊在+1 = +2) 繼續檢查字串
            if (IsMatch(s, p, sIdx, pIdx + 2, dp)) return true;
        }
        if (sIdx == s.Length) return false;
        if (ch == th || th == '.')
        {
            if (pIdx + 1 < p.Length && p[pIdx + 1] == '*')
            {   //「*匹配一次或多次前一個字符」                「* 在匹配當前字符後不再繼續作用」
                return IsMatch(s, p, sIdx + 1, pIdx, dp) || IsMatch(s, p, sIdx + 1, pIdx + 2, dp);
            }
            return IsMatch(s, p, sIdx + 1, pIdx + 1, dp);
        }
        return false;
    }


    public class Regular()
    {
        private string _s;
        public string _p;

        public bool IsMatchHelper(string s, string p)
        {
            _s = s;
            _p = p;
            return IsMatch(0, 0);
        }
        private bool IsMatch(int sIdx, int pIdx)
        {
            // 目的: 用Index去判斷已配對過的
            // 條件: '.'用來代表任何一個字元，'*'代表0或多個前綴字元
            // (1) 如果都是同字元 true
            // (2) p = '.' true if(p[pIdx] == '.') == true
            // (3) p = '*'要去判斷 s[sIdx-1] = s[sIdx] ?? true [匹配零次或多次前一個字符] [它不會自己獨立存在，必須跟在某個字符後面]
            // (4) p = '.*' 代表0或多個字元 true
            // (5) sIdx, pIdx == string.Length return true
            //bool[,] dp = new bool[m + 1, n + 1]; //二維陣列

            // Index到了就返回
            if (sIdx == _s.Length && pIdx == _p.Length) return true;

            //判斷目前可否匹配 (1) (2)
            bool firstMatch = sIdx < _s.Length && pIdx < _p.Length && (_p[pIdx] == '.' || _s[sIdx] == _p[pIdx]);

            //遇到星號'*'，pIdx要在結束前一個，才可以去判斷後面pIdx+1，不然會溢出
            if (pIdx < _p.Length - 1 && _p[pIdx + 1] == '*') //當 _p[pIdx + 1] == '*'遇到星號時，s[sIdx]繼續往前
            {
                //假設不使用星號匹配 跳過星號去匹配 pIdx +2 (因為上面+1這邊在+1 = +2) 繼續檢查字串
                return IsMatch(sIdx, pIdx + 2) || firstMatch && IsMatch(sIdx + 1, pIdx); //'aaaa' == 'a*' => true
            }

            return firstMatch && IsMatch(sIdx + 1, pIdx + 1);
        }
    }
    #endregion

    #region 11. Container With Most Water
    public int MaxArea(int[] height)
    {
        int left = 0, right = height.Length - 1;
        int maxArea = 0;
        while (left < right)
        {
            //Area = Min(height[i], height[j]) * (right - left)  (left < right)

            int temp = Math.Min(height[left], height[right]) * (right - left);
            maxArea = Math.Max(maxArea, temp);

            if (height[left] < height[right]) //因為會取小值，如果左邊值比較小就跳下一個
            {
                left++;
            }
            else //相反右邊值比較小，就往前跳
            {
                right--;
            }
        }
        return maxArea;
    }
    #endregion

    #region 12. Integer to Roman 
    // I=1, V=5, X=10, L=50, C=100, D=500, M=1000
    //4 (IV), 9 (IX), 40 (XL), 90 (XC), 400 (CD) and 900 (CM).
    //10 (I, X, C, M) at most 3 times to represent multiples of 10.
    //You cannot append 5 (V), 50 (L), or 500 (D) multiple times.
    //If you need to append a symbol 4 times use the subtractive form.
    //可以用dp嗎? 上面都是已知條件
    public string IntToRoman(int num)
    { //Greedy
        //var dic = new Dictionary<int, string>
        //{
        //   {1000, "M"}, {900, "CM"}, {500, "D"}, {400, "CD"},
        //{100, "C"}, {90, "XC"}, {50, "L"}, {40, "XL"},
        //{10, "X"}, {9, "IX"}, {5, "V"}, {4, "IV"}, {1, "I"}
        //};
        //var result = new StringBuilder();

        //foreach (var kvp in dic)
        //{
        //    // 盡可能匹配當前最大值
        //    while (num >= kvp.Key)
        //    {
        //        result.Append(kvp.Value);
        //        num -= kvp.Key;
        //    }
        //}

        //return result.ToString();
        var numList = new List<int> { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        var numeralList = new List<string> { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        var result = string.Empty;
        var i = 0;
        while (num > 0)
        {
            var n = num / numList[i];
            if (n != 0)
            {
                result += string.Concat(Enumerable.Repeat(numeralList[i], n));
            }
            num %= numList[i];
            i++;
        }
        return result;
    }
    #endregion

    #region 13. Roman to Integer
    public int RomanToInt(string s)
    {
        //I can be placed before V(5) and X(10) to make 4 and 9.
        //X can be placed before L(50) and C(100) to make 40 and 90.
        //C can be placed before D(500) and M(1000) to make 400 and 900.
        var romanValues = new Dictionary<char, int>
        {{ 'I', 1 },{ 'V', 5 },{ 'X', 10 },{ 'L', 50 },{ 'C', 100 },{ 'D', 500 },{ 'M', 1000 } };
        int result = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int currentValue = romanValues[s[i]];

            //從字頭到字尾，判斷當4或9時，利用小值字母會在前的邏輯
            if (i < s.Length - 1 && currentValue < romanValues[s[i + 1]])
            {
                result -= currentValue; //"IV" = 4，當遍歷到I=1，減掉後是-1，在遍歷到V=5，-1+5=4
            }
            else
            {
                result += currentValue;
            }
        }
        return result;
    }
    #endregion

    #region 14. Longest Common Prefix
    public string LongestCommonPrefix(string[] strs)
    {
        //前綴字元必定是從最前面開始
        //最常見的解法是比較每個字串的字符，直到找出公共前綴。
        string prefix = strs[0]; //假設第一個字是前綴

        for (int i = 1; i < strs.Length; i++)
        {
            // 從最前面開始匹配，如果strs[i]跟prefix預設第一個字元沒有相同的開始字元，直接跳過
            while (!strs[i].StartsWith(prefix))
            {
                prefix = prefix.Substring(0, prefix.Length - 1); //逐個縮短
                if (prefix == "") return "";
            }
        }
        return prefix;
    }
    #endregion

    #region 15. 3Sum
    public IList<IList<int>> ThreeSum(int[] nums)
    {
        //[nums[i], nums[j], nums[k]] such that i != j, i != k, and j != k, and nums[i] + nums[j] + nums[k] == 0
        //三個不同的元素相加等於0，索引都要不同
        //解題思路: 迴圈遍歷，用雙指針在後面找兩個數，相加等於0
        Array.Sort(nums);
        //先排序過後，如果總和小於0，代表數字太小，左指針往右移
        List<IList<int>> numList = new List<IList<int>>();

        for (int i = 0; i < nums.Length - 2; i++)
        {
            if (i > 0 && nums[i] == nums[i - 1])
            {
                //避免兩個重複數字生成重複結果，例如-1,-1,0,1，就會有兩個-1,0,1
                continue;
            }
            int left = i + 1, right = nums.Length - 1;
            while (left < right)
            {
                int sum = nums[i] + nums[left] + nums[right];
                if (sum == 0)
                {
                    numList.Add(new List<int> { nums[i], nums[left], nums[right] });

                    while (left < right && nums[left] == nums[left + 1])
                    {// 跟上面的重複值意思一樣
                        left++;
                    }
                    while (left < right && nums[right] == nums[right - 1])
                    {// 跟上面的重複值意思一樣
                        right--;
                    }
                    left++;
                    right--;
                }
                else if (sum < 0)
                {
                    left++;
                }
                else
                {
                    right--;
                }

            }
        }
        return numList;
    }
    #endregion

    #region 16. 3Sum Closest
    public static int ThreeSumClosest(int[] nums, int target)
    {
        //找出array三者加起來最接近target的組合數字
        //解題思路: 固定第一個數字，用雙指針往後找，一樣先排序，每一個組合都去儲存在一個數字，比較「當前和 (sum) 與目標 (target) 的差距大小」
        Array.Sort(nums);
        // 初始化 closest 為最大值
        int closest = nums[0] + nums[1] + nums[2];
        for (int i = 0; i < nums.Length - 2; i++)
        {
            int left = i + 1, right = nums.Length - 1;
            while (left < right)
            {
                int sum = nums[i] + nums[left] + nums[right];
                if (Math.Abs(sum - target) < Math.Abs(closest - target)) //用初始值去比較大小
                {
                    closest = sum;
                }

                if (sum > target)
                {
                    right--;
                }
                else
                {
                    left++;
                }
            }
        }
        return closest;
    }
    #endregion

    #region 17. Letter Combinations of a Phone Number
    /// 0 <= digits.length <= 4
    /// digits[i] is a digit in the range ['2', '9'].
    /// 每個數字對應的字母是選擇的分支。
    /// 每次從當前數字的字母集合中選擇一個，然後進入下一層數字。
    /// 當遍歷到 digits 的結尾時，當前生成的字母組合是一個完整結果。
    public static IList<string> LetterCombinations(string digits)
    {
        if (string.IsNullOrWhiteSpace(digits)) return new List<string>();
        var result = new List<string>();
        var combination = new StringBuilder();
        var phoneDic = new Dictionary<char, string>()
        {
            { '2', "abc" }, { '3', "def" }, { '4', "ghi" }, { '5', "jkl" }, { '6', "mno" }, { '7', "pqrs" }, { '8', "tuv" }, { '9', "wxyz" }
        };
        BackTrack(0, combination, digits, result, phoneDic);

        return result;
    }
    private static void BackTrack(int index, StringBuilder combination, string digits, List<string> result, Dictionary<char, string> phoneDic)
    {
        if (index == digits.Length)
        {
            result.Add(combination.ToString());
            return;
        }
        string letters = phoneDic[digits[index]];
        foreach (var letter in letters)
        {
            combination.Append(letter);

            BackTrack(index + 1, combination, digits, result, phoneDic);

            combination.Remove(combination.Length - 1, 1); //遞迴逐層返回，一個循環迭代結束就會逐層返回
        }
    }

    #endregion

    #region 18. 4Sum
    //0 <= a, b, c, d < n
    //a, b, c, and d are distinct.
    //nums[a] + nums[b] + nums[c] + nums[d] == target
    //解題思路 : 先排序，固定最前面兩個數字(用for迴圈)，雙指針找第三個數字~最後一個數字，如果相加大於target就讓右邊--，小於就左邊++
    public static IList<IList<int>> FourSum(int[] nums, int target)
    {
        Array.Sort(nums);
        var result = new List<IList<int>>();
        if (nums == null || nums.Length < 4) return result;

        //固定第一個數字，到倒數第三個數字就不足四個數字了所以-3的長度
        for (int i = 0; i < nums.Length - 3; i++)
        {
            //跳過重複數字
            if (i > 0 && nums[i] == nums[i - 1]) continue;
            //固定第二個數字，i是第一個數字剩下需要三個，j是一個所以到倒數第二個數字時就可以停止了，因為不足四個
            for (int j = i + 1; j < nums.Length - 2; j++)
            {
                //跳過重複數字
                if (j > i + 1 && nums[j] == nums[j - 1]) continue;
                //第三個數字跟最後一個數字
                int left = j + 1, right = nums.Length - 1;
                while (left < right)
                {
                    long sum = (long)nums[left] + nums[right] + nums[i] + nums[j]; //LeetCode會測試超過int的值，所以必須改為long
                    if (sum == target)
                    {
                        result.Add(new List<int> { nums[i], nums[j], nums[left], nums[right] });

                        while (left < right && nums[left] == nums[left + 1]) left++;
                        while (left < right && nums[right] == nums[right - 1]) right--;
                        left++;
                        right--;
                    }
                    else if (sum < target)
                    {
                        left++;
                    }
                    else
                    {
                        right--;
                    }

                }
            }
        }

        return result;
    }
    //排序的時間複雜度為 𝑂(𝑛log⁡𝑛)
    //第一層和第二層迴圈是 𝑂(𝑛2)
    //雙指針部分是 𝑂(𝑛)
    //總體時間複雜度仍然為 𝑂(𝑛3)
    #endregion

    #region 19. Remove Nth Node From End of List
    //給定一個LinkList跟n，要去除掉從後面數過來第n個數去除掉
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }
    public static ListNode RemoveNthFromEnd(ListNode head, int n)
    {
        //節點算法沒有0，從1開始
        // 初始化虛擬節點
        ListNode dummy = new ListNode(0, head);

        // 初始化雙指針
        ListNode first = dummy;
        ListNode second = dummy;

        // 讓 first 指針向前移動 n+1 步，這樣 second 指針會停在要刪除節點的前一個節點
        for (int i = 0; i <= n; i++)
        {
            first = first.next; //到達n+1
        }

        // 同時移動 first 和 second，直到 first 到達鏈表末尾
        while (first != null)
        {
            first = first.next; //到尾端
            second = second.next; //到n-1端
        }

        // 刪除節點
        second.next = second.next.next; //跳過節點

        // 返回新的頭節點
        return dummy.next; //跳過0
    }
    #endregion

    #region 20. Valid Parentheses
    // input s，每一個符號有出現都要有完整結尾
    // stack (Last In First Out)
    // 括號的匹配規則是後開的括號要先閉合，這正符合後進先出的特性。
    public static bool IsValid(string s)
    {
        var bracketDic = new Dictionary<char, char> { { '(', ')' }, { '{', '}' }, { '[', ']' } };
        var stack = new Stack<char>();
        foreach (var c in s)
        {
            if (bracketDic.ContainsKey(c))
            {
                stack.Push(c);
            }
            else if (bracketDic.ContainsValue(c))
            {
                //order錯誤 或是 要pop掉的值c裡面沒有
                //Last In First Out 所以最近的Push key = '(' 要先被Pop掉 Value ')' 才有符合Order
                if (stack.Count == 0 || bracketDic[stack.Pop()] != c) return false;
            }
        }
        return stack.Count == 0;
    }
    #endregion

    #region 21. Merge Two Sorted Lists
    // Merge the two lists into one sorted list.
    public static ListNode MergeTwoLists(ListNode list1, ListNode list2)
    {
        ListNode current = new ListNode();
        ListNode result = current;
        while (list1 != null && list2 != null)
        {
            if (list1 != null && list1.val <= list2.val)
            {
                current.next = new ListNode(list1.val);
                current = current.next;
                list1 = list1.next;
            }
            else
            {
                if (list2 != null && list2.val <= list1.val)
                {
                    current.next = new ListNode(list2.val);
                    current = current.next;
                    list2 = list2.next;
                }
            }
        }

        if (list1 != null)
        {
            current.next = list1;
        }
        if (list2 != null)
        {
            current.next = list2;
        }

        return result.next;
    }
    #endregion

    #region 22. Generate Parentheses
    public static IList<string> GenerateParenthesis(int n)
    {

    }
    #endregion

    #region MaxVowels
    public int MaxVowels(string s, int k)
    {
        HashSet<char> vowels = ['a', 'e', 'i', 'o', 'u'];
        int maxVowels = 0, currentVowels = 0;
        for (int i = 0; i < k; i++)
        {
            if (vowels.Contains(s[i]))
            {
                currentVowels++;
            }
        }
        maxVowels = currentVowels;
        for (int i = k; i < s.Length; i++)
        {
            if (vowels.Contains(s[i - k])) //如果最前面的有包含到，就要把count--，繼續查
            {
                currentVowels--;
            }
            if (vowels.Contains(s[i]))
            {
                currentVowels++;
            }

            maxVowels = Math.Max(maxVowels, currentVowels);

            if (maxVowels == k)
            {
                return maxVowels;
            }
        }

        return maxVowels;
    }

    #endregion
    public double FindMaxAverage(int[] nums, int k)
    {
        double sum = 0;
        for (int i = 0; i < k; i++)
        {
            sum += nums[i];
        }
        double maxAvg = sum / k;
        for (int i = k; i < nums.Length; i++) //剩下要跑幾個數字
        {
            sum += nums[i] - nums[i - k]; //減掉最前面的，在加上最後面的
            maxAvg = Math.Max(maxAvg, sum / k);
        }
        return maxAvg;
    }
    public int MaxOperations(int[] nums, int k)
    {
        Array.Sort(nums);
        int left = 0, right = nums.Length - 1, count = 0;
        while (left < right)
        {
            int sum = nums[left] + nums[right];
            if (sum == k)
            {
                count++;
                left++;
                right--;
            }
            else if (sum < k)
            {
                left++;
            }
            else
            {
                right--;
            }
        }
        return count;
    }
    public bool IsSubsequence(string s, string t)
    {
        int i = 0, j = 0;
        while (j < t.Length)
        {
            if (i == s.Length)
            {
                return true;
            }
            if (s[i] == t[j])
            {
                i++;
            }
            j++;
        }
        return i == s.Length;
    }
    public void MoveZeroes(int[] nums)
    {
        int i = 0, j = 0;
        while (j < nums.Length)
        {
            if (nums[j] != 0)
            {
                if (i != j)
                {
                    int temp = nums[i];
                    nums[i] = nums[j];
                    nums[j] = temp;
                }
                i++;
            }
            j++;
        }
    }
    public int Compress(char[] chars)
    {
        int i = 0, write = 0;
        while (i < chars.Length)
        {
            char currentChar = chars[i];
            int count = 0;

            while (i < chars.Length && chars[i] == currentChar)
            {
                count++;
                i++; //紀錄跑到哪個地方
            }
            chars[write] = currentChar;
            write++;
            if (count > 1)
            {
                foreach (char c in count.ToString())
                {
                    chars[write] = c; //只管繼續更新數值，不用管後面剩餘
                    write++; //紀錄數值更新到哪
                }
            }
        }
        return write;
    }
    public bool IncreasingTriplet(int[] nums)
    {
        int first = int.MaxValue, second = int.MaxValue;
        foreach (int num in nums)
        {
            if (num <= first)
            {
                first = num;
            }
            else if (num <= second)
            {
                second = num;
            }
            else
            {
                return true;
            }
        }

        return false;
        //int count = 0;
        //for (int i = 0; i < nums.Length; i++)
        //{
        //    int temp = nums[i];
        //    if (i == nums.Length && temp > nums[i - 1])
        //    {
        //        count++;
        //    }
        //    else if (temp < nums[i + 1])
        //    {
        //        count++;
        //        temp = nums[i + 1];
        //    }
        //    else
        //    {
        //        if (count >= 3)
        //        {
        //            return true;
        //        }
        //        count = 0;
        //    }
        //}
        //if (count >= 3)
        //{
        //    return true;
        //}
        //return false;
    }
    public int[] ProductExceptSelf(int[] nums)
    {
        //for (int i = 0; i < nums.Length; i++)
        //{
        //    int temp = 1;
        //    for (int j = 0; j < nums.Length; j++)
        //    {
        //        if (i != j)
        //        {
        //            temp *= nums[j];
        //        }
        //    }
        //    result[i] = temp;
        //}
        int[] result = new int[nums.Length];
        int[] leftPro = new int[nums.Length];
        leftPro[0] = 1;
        for (int i = 1; i < nums.Length; i++)
        {
            leftPro[i] = leftPro[i - 1] * nums[i - 1];
        }
        int rightPro = 1;
        for (int i = nums.Length - 1; i >= 0; i--)
        {
            result[i] = leftPro[i] * rightPro;
            rightPro *= nums[i];
        }
        return result;
        //int N = nums.Length;
        //int[] ans = new int[N];

        //int prefix = 1;
        //ans[0] = prefix;
        //for (int i = 0; i < N; i++)
        //{
        //    ans[i] = prefix;
        //    prefix *= nums[i];
        //}

        //int postfix = 1;
        //for (int j = N - 1; j >= 0; j--)
        //{
        //    ans[j] *= postfix;
        //    postfix *= nums[j];
        //}

        //return ans;
        // O(n) time
        // O(1) space
    }
    public string ReverseWords(string s)
    {
        var output = s.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Array.Reverse(output);
        string result = string.Join(" ", output);
        return result.Trim();
    }
    public string ReverseVowels(string s)
    {
        #region 自己寫的版本
        //string[] vowels = { "a", "e", "i", "o", "u" };
        //List<char> output = new List<char>();
        //List<int> outputNum = new List<int>();
        //for (int j = 0; j < s.Length; j++)
        //{
        //    if (vowels.Contains(s[j].ToString().ToLower()))
        //    {
        //        output.Add(s[j]);
        //        outputNum.Add(j);
        //    }
        //}
        //outputNum.Reverse();
        //var chars = s.ToCharArray();
        //for (int i = 0; i < output.Count; i++)
        //{
        //    chars[outputNum[i]] = output[i];
        //}
        ////Tostring用於單一字符的轉換，new string適用char[]轉換
        //return new string(chars);
        #endregion
        HashSet<char> vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
        var chars = s.ToCharArray();
        int left = 0, right = chars.Length - 1;
        while (left < right)
        {
            while (left < right && vowels.Contains(chars[left]) == false)
            {
                left++;
            }
            while (left < right && vowels.Contains(chars[right]) == false)
            {
                right--;
            }

            if (left < right)
            {
                char temp = chars[left];
                chars[left] = chars[right];
                chars[right] = temp;
                left++;
                right--;
            }
        }
        return new string(chars);
    }
    public bool CanPlaceFlowers(int[] flowerbed, int n)
    {
        int canPlaceNum = 0;
        for (int i = 0; i < flowerbed.Length; i++)
        {
            if (flowerbed[i] == 0 && (i == 0 || flowerbed[i - 1] == 0) && (i == flowerbed.Length - 1 || flowerbed[i + 1] == 0))
            {
                flowerbed[i] = 1;
                canPlaceNum++;
                i++;//直接在跳過一個，因為相鄰不能種
            }
            if (canPlaceNum >= n)
            {
                return true;
            }
        }
        return canPlaceNum >= n;
    }
    public IList<bool> KidsWithCandies(int[] candies, int extraCandies)
    {
        int maxNum = candies.Max();
        bool[] result = new bool[candies.Length];
        for (int i = 0; i < candies.Length; i++)
        {
            if (candies[i] + extraCandies >= maxNum)
            {
                result[i] = true;
            }
            else
            {
                result[i] = false;
            }
        }
        return result;
    }
    public string GcdOfStrings(string str1, string str2)
    {
        if (str1.Length < str2.Length)
        {
            return GcdOfStrings(str2, str1);
        }
        else if (string.IsNullOrWhiteSpace(str2))
        {
            return str1;
        }
        else if (str1.Substring(0, str2.Length) != str2)
        {
            return "";
        }
        else
        {
            return GcdOfStrings(str1.Substring(str2.Length), str2);
        }
    }
    private bool TwoSquare(int num)
    {
        if (num >= 2)
        {
            return TwoSquare(num / 2);
        }
        else
        {
            return num == 1;
        }
        //while (num % 2 == 0)
        //{
        //    num = num / 2;
        //}
        //return num == 1;
    }

    #region
    //內容替換
    //使用Console.ReadLine() 讀取一個整數 max
    //至少大於 90)90)，在螢幕上用 WriteLine 分行顯示
    //1~max ，其中可被 3 整除者替換為 Build ，可被
    //5 整除者替換為 School ，可以被 3 和 5 同時整
    //除者替換為 Dann 。
    private void ContentSwitch()
    {
        int max = Convert.ToInt32(Console.ReadLine());
        if (max < 90)
        {
            Console.WriteLine("number must be greater than 90");
            return;
        }
        for (int i = max; i > 0; i--)
        {
            if (i % 3 == 0 && i % 5 == 0)
            {
                Console.WriteLine("Build");
            }
            else if (i % 3 == 0)
            {
                Console.WriteLine("School");
            }
            else if (i % 5 == 0)
            {
                Console.WriteLine("Dann");
            }
            else
            {
                Console.WriteLine(i);
            }
        }
        Console.ReadLine();
    }
    #endregion


    #region Fibonacci(DP)
    //Top-Down
    //遞迴的本質導致呼叫堆疊的使用，對於非常大的n，可能會有堆疊溢出（Stack Overflow）的風險。
    //優點：陣列中保存了每個值，方便快速查詢。
    //缺點：對於單純計算最終結果的場合，許多中間結果實際上是不需要的，會造成 記憶體浪費。
    private int Fibonacci(int n)
    {
        var dp = new int[n + 1];
        dp[0] = dp[1] = 1; //設定已知初始值
        if (n > 0)
            dp[1] = 1;
        return FibonacciHelper(n, dp);
    }
    private int FibonacciHelper(int n, int[] dp)
    {
        if (dp[n] > 0) return dp[n]; //有儲存過的直接返回
        dp[n] = FibonacciHelper(n - 1, dp) + FibonacciHelper(n - 2, dp); //儲存進dp陣列
        return dp[n];
    }
    //Bottom-UP
    //僅保存最近兩個斐波那契數字的值，節省了空間。
    //優點：只保留需要的值，節約記憶體。
    //缺點：如果需要得到整個斐波那契序列，而不是單個數值，需要進行額外的處理。
    private int FibonacciButtomUp(int n)
    {
        if (n <= 1)
            return 1;

        int prev = 1, curr = 1;

        for (int i = 2; i <= n; i++)
        {
            int next = prev + curr;
            prev = curr;
            curr = next;
        }

        return curr;
    }

    #endregion
}